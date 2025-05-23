using DataAcsess.Abstract;
using Entities.Concrete;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ECommorceWeb.Controllers
{
    public class invoice : IDocument
    {
 


        private readonly Order _order;
        private readonly CartItem _cartItem;
        private readonly Product _product;
        private readonly ApUser _user;
        private readonly Address _address;
        public invoice(Order orders, CartItem cartItem, Product product, ApUser user, Address address)
        {
          
            _order = orders;
            _cartItem = cartItem;
            _product = product;
            _user = user;
            _address = address;
        }



        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {


            var adres = _address.AddressTitle + _address.City + _address.Street + _address.Neighbourhood;



            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(12));
                page.Content().Column(col =>
                {

                    col.Item().Text($"FATURA").FontSize(20).Bold().AlignCenter();
                    col.Item().Text($"Sipariş No: {_order.OrderId}").SemiBold();
                    col.Item().Text($"Tarih: {_order.OrderDate:dd.MM.yyyy}");
                    col.Item().Text($"Müşteri: {_user.Name + " " + _user.LastName}");
                    col.Item().Text($"Telefon : {_user.PhoneNumber}");
                    col.Item().PaddingVertical(10).LineHorizontal(1);



                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Ürün").Bold();
                            header.Cell().Text("Adet").Bold();
                            header.Cell().Text("Ürün Seçenek").Bold();
                            header.Cell().Text("Fatura Adresi").Bold();

                        });


                        table.Cell().Text(_product.Name);
                        table.Cell().Text(_cartItem.SelectedQuantity.ToString());
                        table.Cell().Text(_cartItem.SelectedValue.ToString());
                        table.Cell().Text(adres);


                    });

                    col.Item().PaddingTop(10).AlignRight().Text(text =>
                    {
                        text.Span("Toplam: ").SemiBold();

                        text.Span(_cartItem.SelectedPrice.ToString("C2") + " ₺").Bold();


                    });

                    col.Item().PaddingTop(20).Text("Bizi Tercih Ettiğiniz için Teşekkür ederiz.").Italic();
                });
            });
        }
    }

}
