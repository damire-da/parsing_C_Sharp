namespace parsing_C_Sharp
{
    class Card
    {
        public Card(string? region, string? name, string? oldPrice, string? price, string? stock, string? imgUrl)
        {
            Region = region;
            Name = name;
            OldPrice = oldPrice;
            Price = price;
            Stock = stock;
            ImgUrl = imgUrl;

        }
        public string Region {get; set;}
        //public string Crumb {get; set;}
        public string Name {get; set;}
        public string OldPrice {get; set;}
        public string Price {get; set;}

        public string Stock {get;set;}

        public string ImgUrl {get;set;}

    }
}