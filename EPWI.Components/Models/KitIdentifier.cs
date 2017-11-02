namespace EPWI.Components.Models
{
	public class KitIdentifier
	{
		private string kitID;
		private string kitType;

    public string AcesKitIdentifier { get; set; }


		public string KitID
		{
			get
			{
				return kitID;
			}
			set
			{
				kitID = value.ToUpper();
			}
		}

		public string KitType
		{
			get
			{
				return kitType;
			}
			set
			{
				kitType = value.ToUpper();
			}
		}

		public string KitPartNumber 
		{
			get
			{
				return KitID + KitType;
			}
		}
	}
}
