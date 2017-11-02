using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace EPWI.Components.Models
{
	public partial class Order
	{
		public Address SoldToAddress
		{
			get
			{
				return new Address { 
					Name = (SoldToName ?? string.Empty).Trim(),
					StreetAddress1 = (SoldToAddress1 ?? string.Empty).Trim(),
					StreetAddress2 = (SoldToAddress2 ?? string.Empty).Trim(),
					City = (SoldToCity ?? string.Empty).Trim(),
					State = (SoldToState ?? string.Empty).Trim(),
					Zip = (SoldToZip ?? string.Empty).Trim(),
					Zip4 = (SoldToZip4 ?? string.Empty).Trim(),
					Phone = (SoldToPhone ?? string.Empty).Trim()
				};
			}
		}

		public Address ShipToAddress
		{
			get
			{
				return new Address
				{
					Name = (ShipToName ?? string.Empty).Trim(),
					StreetAddress1 = (ShipToAddress1 ?? string.Empty).Trim(),
					StreetAddress2 = (ShipToAddress2 ?? string.Empty).Trim(),
					City = (ShipToCity ?? string.Empty).Trim(),
					State = (ShipToState ?? string.Empty).Trim(),
					Zip = (ShipToZip ?? string.Empty).Trim(),
					Zip4 = (ShipToZip4 ?? string.Empty).Trim(),
					Phone = (ShipToPhone ?? string.Empty).Trim()
				};
			}
			set
			{
				ShipToName = value == null ? null : value.Name;
				ShipToAddress1 = value == null ? null : value.StreetAddress1;
				ShipToAddress2 = value == null ? null : value.StreetAddress2;
				ShipToCity = value == null ? null : value.City;
				ShipToState = value == null ? null : value.State;
				ShipToZip = value == null ? null : value.ZipRaw;
				ShipToZip4 = value == null ? null : value.Zip4Raw;
				ShipToPhone = value == null ? null : value.PhoneRaw.Replace(" ", string.Empty).Replace("-", string.Empty).Replace(".", string.Empty);
			}
		}

		public bool HasShipToAddress
		{
			get
			{
				return !string.IsNullOrEmpty(ShipToName);
			}
		}

	  public bool IsPowerUserOrder
	  {
	    get { return !string.IsNullOrEmpty(PrimaryWarehouse) && PrimaryWarehouse.Trim().Length > 0; }
	  }

		public void PopulateFromHost(DataRow orderRow, ICustomerData customerData)
		{			
			InvoiceNumber = int.Parse(orderRow["ZOICN"].ToString());
			BillToCustID = int.Parse(orderRow["ZBCUST"].ToString());
			SoldToName = orderRow["ZCNAM"].ToString();
			SoldToAddress1 = orderRow["ZCAD1"].ToString();
			SoldToAddress2 = orderRow["ZCAD2"].ToString();
			SoldToCity = orderRow["ZCCIT"].ToString();
			SoldToState = orderRow["ZCST"].ToString();
			SoldToZip = orderRow["ZCZIP"].ToString();
			SoldToZip4 = orderRow["ZCZIP4"].ToString();
			SoldToPhone = orderRow["ZCPH"].ToString();
			PORequired = orderRow["ZPO"].ToString().ToCharArray()[0];
			PONumber = orderRow["ZCPO"].ToString().Trim();
			TermsCode = orderRow["ZTERM"].ToString().ToCharArray()[0];
			PriceType = int.Parse(orderRow["ZPTYPE"].ToString());
			CCOnFile = orderRow["ZCC"].ToString().ToCharArray()[0];
			Taxable = orderRow["ZTAX"].ToString().ToCharArray()[0];
			TaxValue = decimal.Parse(orderRow["ZTAX$"].ToString());
			TaxPercent = decimal.Parse(orderRow["ZTAXPC"].ToString());
			SubTotal = decimal.Parse(orderRow["ZMDSE$"].ToString());
			SpecialCharges = decimal.Parse(orderRow["ZSC$"].ToString());
			OrderTotal = decimal.Parse(orderRow["ZORD$"].ToString());
			ShipToName = orderRow["ZSNAM"].ToString().Trim();
			ShipToAddress1 = orderRow["ZSADR"].ToString().Trim();
			ShipToAddress2 = null;
			ShipToCity = orderRow["ZSCIT"].ToString().Trim();
			ShipToState = orderRow["ZSST"].ToString().Trim();
			ShipToZip = orderRow["ZSZIP"].ToString().Trim();
			ShipToZip4 = orderRow["ZSZIP4"].ToString().Trim();
			ShipToPhone = orderRow["ZSPH"].ToString().Trim();
			AllowDropShip = orderRow["ZDS"].ToString() != "N";
			AssignedWhse = orderRow["ZWH"].ToString().Trim();
			PreferredWhse = orderRow["ZPWH"].ToString().Trim();
			DefaultShipMethod = orderRow["ZSV"].ToString().Trim();
			// only update the default ship method name if one is present, because apparently we don't always get it back from the host
			if (orderRow["ZSH"].ToString().Trim() != string.Empty)
			{
				DefaultShipMethodName = orderRow["ZSH"].ToString().Trim();
			}
			// set the requested ship method to the default ship method if blank
			RequestedShipMethod = orderRow["ZSVA"].ToString().Trim() == string.Empty ? DefaultShipMethod : orderRow["ZSVA"].ToString().Trim();
			Comments1 = orderRow["ZCOM1"].ToString().Trim();
			Comments2 = orderRow["ZCOM2"].ToString().Trim();
			AltContact1 = orderRow["ZACT1"].ToString().Trim();
			AltContact2 = orderRow["ZACT2"].ToString().Trim();
			OrderStatus = orderRow["ZOSTS"].ToString().ToCharArray()[0];
			StatusFlag1 = orderRow["ZSTS1"].ToString().ToCharArray()[0];
			StatusFlag2 = orderRow["ZSTS2"].ToString().ToCharArray()[0];
			StatusFlag3 = orderRow["ZSTS3"].ToString().ToCharArray()[0];
			ManualHandling = orderRow["ZMH"].ToString() == "Y";
			ManualReason1 = orderRow["ZMHR1"].ToString().Trim();
			ManualReason2 = orderRow["ZMHR2"].ToString().Trim();
			ManualReason3 = orderRow["ZMHR3"].ToString().Trim();
			ManualReason4 = orderRow["ZMHR4"].ToString().Trim();
			
		}

    public OrderItem MostRecentItem
    {
      get
      {
        return OrderItems.Where(oi => oi.Quantity > 0 && oi.ParentItemID == null).OrderBy(oi => oi.OrderItemID).LastOrDefault();
      }
    }

		public decimal? SubTotalCalculated
		{
			get
			{
				return OrderItems.Where(oi => oi.ZeroPrice == false).Sum(oi => oi.Quantity * oi.DiscountedPrice);
			}
		}

		public string OrderNotes
		{
			get
			{
				return Comments1 + Comments2;
			}
			set
			{
				string orderNotes = value.Replace("\r\n", " ");

				Comments1 = orderNotes.Substring(0, Math.Min(60, orderNotes.Length));

				if (orderNotes.Length > 60)
				{
					Comments2 = orderNotes.Substring(60, Math.Min(orderNotes.Length - 60, 60));
				}
				else
				{
					Comments2 = string.Empty;
				}
			}
		}


		public string OrderStatusMessage
		{
			get
			{
				switch (OrderStatus.GetValueOrDefault(' '))
				{
					case 'E':
						return "This Order Has An Error";
					case 'W':
						return "There Are Inventory Availability Errors";
					default:
						return "An Unknown Error Has Occurred";
				}
			}
		}

		public string GetStatusFlagMessage(int statusFlag)
		{
			char code = ' ';
			string message = null;

			if (!(statusFlag > 0 && statusFlag < 4))
			{
				throw new ArgumentOutOfRangeException("statusFlag must be between 1 and 3");
			}

			if (statusFlag == 1)
			{
				code = StatusFlag1.GetValueOrDefault(' ');
				switch (code)
				{
					case 'P':
						message = "A Purchase Order is Required To Process This Order";
						break;
					case 'C':
						message = "The Customer Is Invalid Or Not Found In The System";
						break;
				}
			}
			else if (statusFlag == 2)
			{
				code = StatusFlag2.GetValueOrDefault(' ');
				switch (code)
				{
					case 'T':
						message = "Invalid Terms Code";
						break;
					case 'C':
						message = "Terms Code Mismatch";
						break;
				}
			}
			else if (statusFlag == 3)
			{
				code = StatusFlag3.GetValueOrDefault(' ');
				switch (code)
				{
					case 'W':
						message = "Invalid Warehouse Code";
						break;
					case 'X':
						message = "Invalid Warehouse Assignment";
						break;
					case 'S':
						message = "Invalid Alternate Ship Method";
						break;
				}
			}

			if (message != null)
			{
				return $"STS{statusFlag}={code}: {message}";
			}
			
			return string.Empty;
		}

		public IEnumerable<string> GetOrderItemMessages()
		{
			var messages = new List<string>();

			int counter = 1;

			foreach (OrderItem oi in OrderedOrderItems)
			{
				string message;

				if (oi.ErrorFlag.GetValueOrDefault(' ') != ' ')
				{
					switch (oi.ErrorFlag.Value)
					{
						case 'A':
							message = "Has Inventory Available At Primary Warehouse. Click \"Change\" on the item to check inventory availability.";
							break;
						case 'S':
              message = "Has Insufficient Quantity At Shipping Warehouse. Click \"Change\" on the item to check inventory availability.";
							break;
						case 'I':
							message = "Has No Match In Inventory - Please Contact EPWI";
							break;
						case 'P': // if there is a "P" error, the order wizard has updated pricing
							message = "Has An Updated Price. Please review the new price before submitting your order.";
							break;
						case 'W':
							message = "Has A Warehouse Error - Please Contact EPWI";
							break;
						case 'X':
							message = "Has An Invalid Order Method Code - Please Contact EPWI";
							break;
						default:
							message = "Has An Unknown Error - Please Correct Before Processing";
							break;
					}
					message = $"Item #{counter} {message}";
					messages.Add(message);
				}

				counter++;
			}

			return messages;
		}

		public IEnumerable<OrderItem> OrderedOrderItems
		{
			get
			{
				return from oi in OrderItems
										where oi.Quantity > 0
										orderby oi.SequenceNumber
										select oi;
			}
		}

		public void SetManualFlags()
		{
			// check if more than three characters exist in the notes fields (three characters allows for a couple of random keys accidentally hit)
			if (Comments1.Trim().Length > 3 || Comments2.Trim().Length > 3)
			{
				setManualFlag("COM");
			}

			if (DefaultShipMethod != RequestedShipMethod)
			{
				setManualFlag("SV");
			}

			// determine if the order method of any item will flag the order as manual
			char[] manualOrderMethods ={ 'L', 'D', 'X' };
			if (OrderItems.Any(oi => manualOrderMethods.Contains(oi.OrderMethod.GetValueOrDefault(' '))))
			{
				setManualFlag("OM");
			}

			// flag order if more than 2 items are to be shipped from a warehouse other than the user's assigned warehouse
			var moreThanTwoOtherWarehouses = (from oi in OrderItems
																				 where oi.Warehouse != AssignedWhse
																				 select oi).Count() > 2;
			if (moreThanTwoOtherWarehouses)
			{
				setManualFlag("MSL");
			}

      // Power user checks
		  if (IsPowerUserOrder)
		  {
        // if primary warehouse is ANC and any items are shipping from elsewhere need to set manual flag
		    if (PrimaryWarehouse == "ANC" && OrderItems.Any(oi => oi.Warehouse != "ANC"))
		    {
		      setManualFlag("FRT");
		    } 
        // if order is shipping to HI, need to set manual flag
        if (ShipToState == "HI")
        {
          setManualFlag("FRT");
        }
		  }
		}

		private void setManualFlag(string flag)
		{
			flag = flag.Trim();

			if (flag.Length > 3)
			{
				throw new ArgumentOutOfRangeException("flag", "flag must be 3 characters or less");
			}

			string[] currentFlags = { ManualReason1.Trim(), ManualReason2.Trim(), ManualReason3.Trim(), ManualReason4.Trim() };

			// only set the flag if it isn't already set
			if (!currentFlags.Contains(flag))
			{
				// loop through each of the 4 manual handling reason fields and put the reason in the first one that is blank
				// if all of them are full, then don't worry about it
				for (int i = 0; i < 4; i++)
				{
					if (string.IsNullOrEmpty(currentFlags[i]))
					{
						var pi = GetType().GetProperty("ManualReason" + (i + 1));
						pi.SetValue(this, flag, null);
						break;
					}
				}
			}

			ManualHandling = true;
		}		
	}
}
