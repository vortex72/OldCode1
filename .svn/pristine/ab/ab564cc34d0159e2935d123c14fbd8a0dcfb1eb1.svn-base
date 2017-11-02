





ALTER PROCEDURE [dbo].[usp_GetKitCatalogMasterKitParts]
/* 
***********************************************************************************************
Author:			  Brian McCleary
Application:	EPWI
Date: 			  09/20/2004
Purpose:		  Rewritten version of the master kit parts routine based on logic prepared by
              Jay Dahl in his email dated 9/14/2004 with attachment named 'Kits02.doc'
Revisions:	  
              09/12/2005 - Added @iSelectedMaskGroupSeq to allow for KTRACKs grouping
                Possible values for @iSelectedMaskGroupSeq
                  -1 : Return only the "OR" groups for KTRACKs
                  0 : Display kit as normal
                  Sequence Number (>0) : Return all AND'ed parts for a given KTRACK grouping identified by the sequence number

			10.12.2016 (EB) EPWI-165: Modified to Order by SequenceNumber - it appears that the as400 sync links 
				perform a bit differently on Azure than they did on SQL 2008.
***********************************************************************************************
*/
--Parameters
@sKitID char(10),
@sKitType varchar(3),
@iYear int, -- This is optional, pass a value of 0 to eliminate any year filter
@iSelectedMaskGroupSeq int -- This is optional.  Set to 0 unless, for a kit has special masking (for KTRACKs), then pass in the sequence number of the first item in the group that will be returned.  If the kit requires special display and the value is set to 0, then only the main grouping items (the OR's) will be returned.

AS

SET NOCOUNT ON

-- Determine the Correct code to use for the KTYPE field in KITHPC
DECLARE @sKitTypeCode char(1)
SELECT @sKitTypeCode = 
  CASE @sKitType
    WHEN 'CK' THEN 'C'
    WHEN 'EK' THEN 'N'
    WHEN 'RMK' THEN 'B'
    WHEN 'RR' THEN 'R'
    WHEN 'RRP' THEN 'W'
    WHEN '*' THEN '*'
    ELSE 'M'
  END

/*
Generate the full SQL that generates the full kit part listing for 
the master kit without any year filtering
*/
SELECT 
  InvMaster.NIPC AS NIPC, 
  LineDescs.LINED AS MfrName, 
  LineDescs.LINE AS MfrCode, 
  InvMaster.ITEM AS PartNum, 
  InvMaster.IDESC AS PartDesc, 
  KitParts.KPPPPC As PricingPercentage,
  KitParts.KPTOP AS JoinQualifier, 
  0 AS GroupingMain,
  0 AS GroupingOr,
  0 AS GroupingAnd,
  Cast(NULL AS VarChar(400)) AS PartsToGroup,
  Cast(NULL AS VarChar(400)) AS PartsToDeselect,
  Cast(NULL AS VarChar(400)) AS PartsToSelect,
  KitParts.KSEQPC AS SequenceNumber, 
  InvMaster.CATGPC AS Category, 
  cc.CategoryID AS CategoryID,
  KitParts.KPQRPC AS QuantityReq,
  (SELECT 
    MIN(IsNull(tbl_kitcat_KITPPC.KPSYY, 0))
    FROM tbl_kitcat_KITPPC INNER JOIN
    tbl_kitcat_KITHPC ON tbl_kitcat_KITPPC.KNIPC = tbl_kitcat_KITHPC.KNIPC
    WHERE (tbl_kitcat_KITPPC.KPNIPC = KitParts.KPNIPC) AND (tbl_kitcat_KITHPC.KNIPC = KitHeader.KNIPC)
  ) AS StartYear,
  (SELECT
    MAX(IsNull(tbl_kitcat_KITPPC.KPEYY, 0))
    FROM tbl_kitcat_KITPPC INNER JOIN
    tbl_kitcat_KITHPC ON tbl_kitcat_KITPPC.KNIPC = tbl_kitcat_KITHPC.KNIPC
    WHERE (tbl_kitcat_KITPPC.KPNIPC = KitParts.KPNIPC) AND (tbl_kitcat_KITHPC.KNIPC = KitHeader.KNIPC)
  ) AS EndYear,
  (SELECT
    MIN(IsNull(tbl_kitcat_KITPNPC.KPNOTE, ''))
    FROM tbl_kitcat_KITHPC INNER JOIN
    tbl_kitcat_KITPPC ON tbl_kitcat_KITHPC.KNIPC = tbl_kitcat_KITPPC.KNIPC INNER JOIN
    tbl_kitcat_KITPNPC ON tbl_kitcat_KITPPC.KNIPC = tbl_kitcat_KITPNPC.KNIPC
    WHERE (tbl_kitcat_KITPNPC.SPNIPC = KitParts.KPNIPC) AND (tbl_kitcat_KITHPC.KNIPC = KitHeader.KNIPC)
   ) AS PartNote

INTO #KitParts

FROM
  tbl_kitcat_KITHPC KitHeader INNER JOIN
  tbl_kitcat_KITPPC KitParts ON KitHeader.KNIPC = KitParts.KNIPC INNER JOIN
  tbl_kitcat_INVMSPC InvMaster ON KitParts.KPNIPC = InvMaster.NIPC INNER JOIN
  tbl_kitcat_ILDESCPF LineDescs ON InvMaster.LINE = LineDescs.LINE  LEFT OUTER JOIN
  CategoryCode cc ON cc.CategoryCode = InvMaster.CATGPC
WHERE
  ((KitHeader.KITHR = @sKitID) AND (KitHeader.KTYPE = '*')) -- This line is for custom (non-engine kit) kits
  OR
  (KitHeader.KITHR = RTRIM(@sKitID) + @sKitType)
ORDER BY 
  KitParts.KNIPC, 
  KitParts.KSEQPC, 
  KitParts.KPNIPC

---------------------------------------------------------

/*
Generate two new tables with absolutly no records, but with exactly the same
structure as the unfiltered parts list (by selecting where KNIPC = NULL)
*/

SELECT * INTO #WorkingParts FROM #KitParts WHERE NIPC = NULL
SELECT * INTO #FinalParts FROM #KitParts WHERE NIPC = NULL

---------------------------------------------------------

/*
If no year is provided, then simply copy all the records from #KitParts to #FinalParts.
Otherwise, we need to perform the complex logic to filter per year while still keeping
the and/or logic in place.

The notes following the lines correspond to the PSEUDO-CODE line declarations
provided by Jay Dahl.  Lines 10 & 20 are taken care of above when creating the table.
*/

IF @iYear = 0
BEGIN
  INSERT INTO #FinalParts SELECT * FROM #KitParts

END
ELSE
BEGIN
  /*
  These flags are used to determine if the records in the working table
  are valid and if so have other "or'ed" records
  */
  DECLARE @AreWorkingItemsValid bit -- Jay referres to this as FLAG
  DECLARE @IsOrActive bit -- Jay referres to this as ORACTIVE
  SET @AreWorkingItemsValid = 0 --: 30
  SET @IsOrActive = 0 --: 40
  
  /*
  These fields are the only ones required for the business logic to 
  filter the records
  */
  DECLARE @SequenceNumber smallint
  DECLARE @JoinQualifier char(1)
  DECLARE @StartYear datetime
  DECLARE @EndYear datetime

  /*
  Utilize the SequenceNumber as a primary key (which it is per kit) and 
  add each record from the main kit parts table to a working table that 
  temporarily holds the record data while determining if any and/or logic 
  needs applied.  Once the records are determined to be valid based on a 
  combination of year and and/or logic, then add the records to the final 
  output table
  */
  DECLARE parts_cursor CURSOR FOR SELECT SequenceNumber, JoinQualifier, StartYear, EndYear FROM #KitParts
  OPEN parts_cursor
  FETCH NEXT FROM parts_cursor INTO @SequenceNumber, @JoinQualifier, @StartYear, @EndYear --: 50
  WHILE @@FETCH_STATUS = 0 --: 60
  BEGIN

    IF (IsNull(@JoinQualifier,'') = '') --: 70
    BEGIN
      SET @IsOrActive = 0 --: 80
    END  --: 90

    IF (@JoinQualifier = 'O') OR (IsNull(@JoinQualifier,'') = '') --: 100
    BEGIN

      IF @AreWorkingItemsValid = 1 --: 110
      BEGIN
        INSERT INTO #FinalParts SELECT * FROM #WorkingParts --: 120
        DELETE FROM #WorkingParts --: 130
        SET @AreWorkingItemsValid = 0 --: 140
      END --: 150

      IF (@StartYear <= @iYear) AND (@iYear <= @EndYear) --: 160
      BEGIN
        INSERT INTO #WorkingParts SELECT * FROM #KitParts WHERE SequenceNumber = @SequenceNumber --: 170
        IF @IsOrActive = 0  --: 170 NOTE
          UPDATE #WorkingParts SET JoinQualifier = NULL WHERE SequenceNumber = @SequenceNumber
        IF (@IsOrActive = 1) AND (IsNull(@JoinQualifier,'') = '') --: 170 NOTE
          UPDATE #WorkingParts SET JoinQualifier = 'O' WHERE SequenceNumber = @SequenceNumber
        SET @AreWorkingItemsValid = 1 --: 180
        SET @IsOrActive = 1 --: 190
      END --: 200

      -- This line filters the result so that only selected "OR" group parts are returned
      IF (@iSelectedMaskGroupSeq > 0) AND (@iSelectedMaskGroupSeq <> @SequenceNumber)
        SET @AreWorkingItemsValid = 0

    END --: 210
  
    IF (@JoinQualifier = 'A') --: 220
    BEGIN

      -- This line eliminates all "AND'ed" items if special display masking is required    
      IF (@iSelectedMaskGroupSeq >= 0)
      BEGIN

        IF (@AreWorkingItemsValid = 1) AND (@StartYear <= @iYear) AND (@iYear <= @EndYear) --: 230
        BEGIN
          INSERT INTO #WorkingParts SELECT * FROM #KitParts WHERE SequenceNumber = @SequenceNumber --: 240
        END --: 250

      END

      IF (@AreWorkingItemsValid = 0) OR (@iYear < @StartYear) OR (@iYear > @EndYear) --: 260
      BEGIN
        DELETE FROM #WorkingParts --: 270
  --      IF (@AreWorkingItemsValid = 0)
  --      BEGIN
		--  --SET @IsOrActive = 0 --: 285
		--END
        SET @AreWorkingItemsValid = 0 --: 280
      END --: 290
    END --: 300

    FETCH NEXT FROM parts_cursor INTO @SequenceNumber, @JoinQualifier, @StartYear, @EndYear  --: 310
  END
  CLOSE parts_cursor
  DEALLOCATE parts_cursor
  
  IF @AreWorkingItemsValid = 1 --: 320
    INSERT INTO #FinalParts SELECT * FROM #WorkingParts --: 330

END --: 340

IF @iYear > 0
BEGIN
  -- Make a second pass to remove any joins between parts of different categories
  -- This can happen if a year filter filters out an or'd part
  UPDATE #FinalParts SET JoinQualifier = NULL WHERE JoinQualifier IS NOT NULL AND SequenceNumber IN
    (SELECT SequenceNumber FROM #FinalParts f WHERE
     CategoryID <> (SELECT TOP 1 CategoryID FROM #FinalParts f2 WHERE f2.SequenceNumber < f.SequenceNumber ORDER BY f2.SequenceNumber DESC))

  --------------------------------------------------------
  -- Generate the grouping codes
  
  -- NIPC code has not been declared yet, so do it here
  DECLARE @NIPCCode int
  
  DECLARE @GroupingMain int
  DECLARE @GroupingOr int
  DECLARE @GroupingAnd int
  SET @GroupingMain = 0
  SET @GroupingOr = 0
  SET @GroupingAnd = 0 
  
  DECLARE grouping_cursor CURSOR FOR SELECT NIPC, SequenceNumber, JoinQualifier FROM #FinalParts Order by SequenceNumber
  OPEN grouping_cursor
  FETCH NEXT FROM grouping_cursor INTO @NIPCCode, @SequenceNumber, @JoinQualifier
  WHILE @@FETCH_STATUS = 0
  BEGIN
    -- Base all grouping logic on the value in the JoinQualifier
  
    IF @JoinQualifier IS NULL
    BEGIN
      -- If JoinQualifier is blank, then increment the main group counter and reset the AND/OR group counters
      SET @GroupingMain = @GroupingMain + 1
      SET @GroupingOr = 1
      SET @GroupingAnd = 1
    END
  
    IF @JoinQualifier = 'O'
    BEGIN
      -- If JoinQualifier is an "O" then increment the OR group counter and reset the AND counter
      SET @GroupingOr = @GroupingOr + 1
      SET @GroupingAnd = 1
    END
  
    IF @JoinQualifier = 'A'
    BEGIN
      -- If JoinQualifier is an "A" then simply increment the AND group counter 
      SET @GroupingAnd = @GroupingAnd + 1
    END
  
    -- Now that we know the appropriate groupings, update the FinalParts table
    UPDATE #FinalParts 
    SET 
      GroupingMain = @GroupingMain, 
      GroupingOr = @GroupingOr, 
      GroupingAnd = @GroupingAnd
    WHERE (NIPC = @NIPCCode) AND (SequenceNumber = @SequenceNumber)
  
    FETCH NEXT FROM grouping_cursor INTO @NIPCCode, @SequenceNumber, @JoinQualifier
  END
  CLOSE grouping_cursor
  DEALLOCATE grouping_cursor
  
  --------------------------------------------------------
  -- Generate the grouping strings
  
  DECLARE @PartsToGroup varchar(400)
  DECLARE @PartsToDeselect varchar(400)
  DECLARE @PartsToSelect varchar(400)
  DECLARE @UniqueID varchar(100) -- The NIPCCode and SequenceNumber combined seperated by a hyphen

  DECLARE grouping_cursor CURSOR FOR SELECT NIPC, SequenceNumber, JoinQualifier, GroupingMain, GroupingOr, GroupingAnd FROM #FinalParts
  OPEN grouping_cursor
  FETCH NEXT FROM grouping_cursor INTO @NIPCCode, @SequenceNumber, @JoinQualifier, @GroupingMain, @GroupingOr, @GroupingAnd
  WHILE @@FETCH_STATUS = 0
  BEGIN
    SET @UniqueID = CAST(@NIPCCode AS varchar(10)) + '-' + CAST(@SequenceNumber AS varchar(3)) 
  
    -- Reset all the grouping strings
    SET @PartsToGroup = ''
    SET @PartsToDeselect = ''
    SET @PartsToSelect = ''
  
    -- Determine which parts are part of what group and what parts need selected or deselected
    SELECT @PartsToGroup = COALESCE(@PartsToGroup + ',', '') + CAST(NIPC AS varchar(10)) + '-' + CAST(SequenceNumber AS varchar(3)) 
      FROM #FinalParts WHERE (GroupingMain = @GroupingMain) AND (CAST(NIPC AS varchar(10)) + '-' + CAST(SequenceNumber AS varchar(3)) <> @UniqueID) ORDER BY SequenceNumber
    IF LEFT(@PartsToGroup, 1) = ',' SET @PartsToGroup = RIGHT(@PartsToGroup, LEN(@PartsToGroup) - 1)

    SELECT @PartsToSelect = COALESCE(@PartsToSelect + ',', '') + CAST(NIPC AS varchar(10)) + '-' + CAST(SequenceNumber AS varchar(3)) 
      FROM #FinalParts WHERE (GroupingMain = @GroupingMain) AND (GroupingOr = @GroupingOr) AND (CAST(NIPC AS varchar(10)) + '-' + CAST(SequenceNumber AS varchar(3)) <> @UniqueID) ORDER BY SequenceNumber
    IF LEFT(@PartsToSelect, 1) = ',' SET @PartsToSelect = RIGHT(@PartsToSelect, LEN(@PartsToSelect) - 1)

    SELECT @PartsToDeselect = COALESCE(@PartsToDeselect + ',', '') + CAST(NIPC AS varchar(10)) + '-' + CAST(SequenceNumber AS varchar(3)) 
      FROM #FinalParts WHERE (GroupingMain = @GroupingMain) AND (GroupingOr <> @GroupingOr) AND (CAST(NIPC AS varchar(10)) + '-' + CAST(SequenceNumber AS varchar(3)) <> @UniqueID) ORDER BY SequenceNumber
    IF LEFT(@PartsToDeselect, 1) = ',' SET @PartsToDeselect = RIGHT(@PartsToDeselect, LEN(@PartsToDeselect) - 1)

    -- Now that we know the appropriate groupings, update the FinalParts table
    UPDATE #FinalParts 
    SET 
      PartsToGroup = @PartsToGroup, 
      PartsToDeselect = @PartsToDeselect, 
      PartsToSelect = @PartsToSelect
    WHERE (NIPC = @NIPCCode) AND (SequenceNumber = @SequenceNumber)
  
    FETCH NEXT FROM grouping_cursor INTO @NIPCCode, @SequenceNumber, @JoinQualifier, @GroupingMain, @GroupingOr, @GroupingAnd
  END
  CLOSE grouping_cursor
  DEALLOCATE grouping_cursor

END -- IF @iYear = 0

--------------------------------------------------------

-- Add the PreJoinQualifier to the selection and return the result set
SELECT 
  NIPC, SequenceNumber, 
  MfrName, MfrCode, PartNum, PartDesc, PricingPercentage,
  JoinQualifier, (SELECT TOP 1 JoinQualifier FROM #FinalParts WHERE SequenceNumber > FP.SequenceNumber ORDER BY SequenceNumber) AS PreJoinQualifier,
  GroupingMain, GroupingOr, GroupingAnd, 
  PartsToGroup, PartsToDeselect, PartsToSelect,
  Category, QuantityReq, StartYear, EndYear, PartNote
FROM #FinalParts FP
Order by SequenceNumber

---------------------------------------------------------

-- Delete the temporary working tables
DROP TABLE #FinalParts
DROP TABLE #WorkingParts
DROP TABLE #KitParts


SET NOCOUNT OFF

RETURN