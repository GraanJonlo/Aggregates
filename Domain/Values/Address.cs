namespace Domain.Values
{
    public readonly struct Address
    {
        public readonly string Address1;
        public readonly string Address2;
        public readonly string City;
        public readonly string County;
        public readonly string Postcode;

        public Address(string address1, string address2, string city, string county, string postcode)
        {
            Address1 = address1;
            Address2 = address2;
            City = city;
            County = county;
            Postcode = postcode;
        }
    }
}