<%
Dim iMapCount

' --- INVMSPF ---
Dim aINVMSPFMap(11,1) ' Approx. 230,000 records
iMapCount = 0
AddDataMap aINVMSPFMap, "NI",     "NIPC"
AddDataMap aINVMSPFMap, "MPRSEQ", "MPRSEP"
AddDataMap aINVMSPFMap, "MPSSEQ", "N/A"
AddDataMap aINVMSPFMap, "USTSEQ", "N/A"
AddDataMap aINVMSPFMap, "OMULT",  "OMULTP"
AddDataMap aINVMSPFMap, "SMULT",  "SMULTP"
AddDataMap aINVMSPFMap, "EMULT",  "EMULTP"
AddDataMap aINVMSPFMap, "CATG",   "CATGPC"
AddDataMap aINVMSPFMap, "OCATG",  "N/A"
AddDataMap aINVMSPFMap, "CITEM",  "N/A"
AddDataMap aINVMSPFMap, "IMCHG",  "N/A"
AddDataMap aINVMSPFMap, "IMAMFG", "IMAMF"

' --- INVMS1PF ---
Dim aINVMS1PFMap(12,1) 
iMapCount = 0
AddDataMap aINVMS1PFMap, "NI",     "NIPC"
AddDataMap aINVMS1PFMap, "IMF09", "IMFL09"
AddDataMap aINVMS1PFMap, "IMF10", "IMFL10"
AddDataMap aINVMS1PFMap, "IMF11", "IMFL11"
AddDataMap aINVMS1PFMap, "IMF12", "IMFL12"
AddDataMap aINVMS1PFMap, "IMF13", "IMFL13"
AddDataMap aINVMS1PFMap, "IMF14", "IMFL14"
AddDataMap aINVMS1PFMap, "IMF15", "IMFL15"
AddDataMap aINVMS1PFMap, "IMF16", "IMFL16"
AddDataMap aINVMS1PFMap, "IMF17", "IMFL17"
AddDataMap aINVMS1PFMap, "IMF18", "IMFL18"
AddDataMap aINVMS1PFMap, "IMF19", "IMFL19"
AddDataMap aINVMS1PFMap, "IMF20", "IMFL20"

' --- INVMSPC ---
'Dim aINVMSPCMap(25,1)
'iMapCount = 0
'AddDataMap aINVMSPCMap, "ITEMPC", "ITEM"
'AddDataMap aINVMSPCMap, "LINEPC", "LINE"
'AddDataMap aINVMSPCMap, "SIZTPC", "SIZTYP"
'AddDataMap aINVMSPCMap, "COREPC", "CORE"
'AddDataMap aINVMSPCMap, "IMTEPC", "IMTE"
'AddDataMap aINVMSPCMap, "IDESCP", "IDESC"
'AddDataMap aINVMSPCMap, "IMFL1P", "IMFL1"
'AddDataMap aINVMSPCMap, "IMFL2P", "IMFL2"
'AddDataMap aINVMSPCMap, "IMFL3P", "IMFL3"
'AddDataMap aINVMSPCMap, "IMFL4P", "IMFL4"
'AddDataMap aINVMSPCMap, "IMFL5P", "IMFL5"
'AddDataMap aINVMSPCMap, "IMFL6P", "IMFL6"
'AddDataMap aINVMSPCMap, "IMFL7P", "IMFL7"
'AddDataMap aINVMSPCMap, "IMFL8P", "IMFL8"
'AddDataMap aINVMSPCMap, "IMDELP", "IMDEL"
'AddDataMap aINVMSPCMap, "IMAMFP", "IMAMF"
'AddDataMap aINVMSPCMap, "CYMD",   "N/A"
'AddDataMap aINVMSPCMap, "CTIME",  "N/A"

' --- KITHPF ---
Dim aKITHPFMap(5,1) ' Approx. 3,200 Records
iMapCount = 0
AddDataMap aKITHPFMap, "KIT#",   "KITHR"
AddDataMap aKITHPFMap, "KNI",    "KNIPC"
AddDataMap aKITHPFMap, "KPHLA",  "N/A"
AddDataMap aKITHPFMap, "KPP1PC", "N/A"
AddDataMap aKITHPFMap, "KPP2PC", "N/A"
AddDataMap aKITHPFMap, "KPP3PC", "N/A"

' --- KITPPF ---
Dim aKITPPFMap(5,1) ' Approx. 20,400 Records
iMapCount = 0
AddDataMap aKITPPFMap, "KNI",  "KNIPC"
AddDataMap aKITPPFMap, "KSEQ", "KSEQPC"
AddDataMap aKITPPFMap, "KPNI", "KPNIPC"
AddDataMap aKITPPFMap, "KPQR", "KPQRPC"
AddDataMap aKITPPFMap, "KPPP", "KPPPPC"
AddDataMap aKITPPFMap, "KPMP", "KPMPPC"

' --- KITPNPF --- ' Kit Part Notes
Dim aKITPNPFMap(3,1) ' Approx. 4,000 Records
iMapCount = 0
AddDataMap aKITPNPFMap, "KNI",  "KNIPC"
AddDataMap aKITPNPFMap, "KPNI", "SPNIPC"
AddDataMap aKITPNPFMap, "KPSEQ", "KPSEQP"
'AddDataMap aKITPNPFMap, "KPNOTE", "KPNOTE"

' --- ILDESCPF ---
Dim aILDESCPFMap(0,1) ' Approx. 264 Records
iMapCount = 0

' --- KZENGPF ---
Dim aKZENGPFMap(0,1) ' Approx. 430 Records
iMapCount = 0

' --- KZKTNPF ---
Dim aKZKTNPFMap(0,1) ' Approx. 972 Records
iMapCount = 0

' --- KZRELPF ---
Dim aKZRELPFMap(4,1) ' Approx. 7,257 Records
iMapCount = 0
AddDataMap aKZRELPFMap, "KSEQ", "KSEQPC"
AddDataMap aKZRELPFMap, "KPNI", "KPNIPC"
AddDataMap aKZRELPFMap, "KPQR", "KPQRPC"
AddDataMap aKZRELPFMap, "KPPP", "KPPPPC"
AddDataMap aKZRELPFMap, "CATG", "CATGPC"

' --- OLCENPF ---
Dim aOLCENPFMap(1,1) ' Approx. 550 Records
iMapCount = 0
AddDataMap aOLCENPFMap, "OCATP",  "OCATPC"
AddDataMap aOLCENPFMap, "OLDENG", "N/A"

' --- OLCMKPF ---
Dim aOLCMKPFMap(0,1) ' Approx. 53 Records
iMapCount = 0

' --- OLCECPF ---
Dim aOLCECPFMap(1,1) ' Approx. 3,600 Records
iMapCount = 0
AddDataMap aOLCECPFMap, "OECSEQ", "OECSPC"

' --- OLCPTNPF ---
Dim aOLCPTNPFMap(1,1) ' Approx. 39,000 Records
iMapCount = 0
AddDataMap aOLCPTNPFMap, "OPNI",   "OPNIPC"
AddDataMap aOLCPTNPFMap, "OPTSEQ", "OPTSEP"

' --- SIZEUPC ---
Dim aSIZEUPCMap(0,1) ' Approx. 35,000 Records
iMapCount = 0
AddDataMap aSIZEUPCMap, "SIZE", "ISIZE"

' --- AIKITHPF ---
Dim aAIKITHPFMap(9,1) ' ACES Kit Header
iMapCount = 0
AddDataMap aAIKITHPFMap, "KIT#",   "KITHR"
AddDataMap aAIKITHPFMap, "KNI",    "KNIPC"
AddDataMap aAIKITHPFMap, "KPHLA",  "N/A"
AddDataMap aAIKITHPFMap, "KPHLA",  "N/A"
AddDataMap aAIKITHPFMap, "KPP1PC", "N/A"
AddDataMap aAIKITHPFMap, "KPP2PC", "N/A"
AddDataMap aAIKITHPFMap, "KPP3PC", "N/A"
'AddDataMap aAIKITHPFMap, "KBNI",   "N/A"
AddDataMap aAIKITHPFMap, "KBKIT#", "N/A"
AddDataMap aAIKITHPFMap, "KPFLAG", "N/A"

' --- AIKITPPF ---
Dim aAIKITPPFMap(8,1) ' ACES Kit Parts
iMapCount = 0
AddDataMap aAIKITPPFMap, "KIT#", "KITHR"
AddDataMap aAIKITPPFMap, "KNI",  "KNIPC"
AddDataMap aAIKITPPFMap, "KSEQ", "KSEQPC"
AddDataMap aAIKITPPFMap, "KPNI", "KPNIPC"
AddDataMap aAIKITPPFMap, "KPQR", "KPQRPC"
AddDataMap aAIKITPPFMap, "KPPP", "KPPPPC"
AddDataMap aAIKITPPFMap, "KPMP", "KPMPPC"
AddDataMap aAIKITPPFMap, "KBKIT#", "N/A"
'AddDataMap aAIKITPPFMap, "KBNI", "N/A"

' --- ZSHPVPF ---
Dim aZSHPVPFMap(3,1) ' Warehouse shipping methods, Approx. 100 Records
iMapCount = 0
AddDataMap aZSHPVPFMap, "ZSWHSE", "WHSE"
AddDataMap aZSHPVPFMap, "ZSSEQ", "SEQ"
AddDataMap aZSHPVPFMap, "ZSVC", "VC"
AddDataMap aZSHPVPFMap, "ZSVIA", "VIA"


'---------------------------------------------------

Function AddDataMap(aFieldArray,OrigField,DestField)
  aFieldArray(iMapCount,0) = OrigField
  aFieldArray(iMapCount,1) = DestField
  iMapCount = iMapCount + 1
End Function

%>