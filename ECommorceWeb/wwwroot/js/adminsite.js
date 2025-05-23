$(document).ready(function () {
    // İlk event handler: admin-btn tıklama
    $('a#admin-btn').on('click', function (e) {
        // Önceki tüm aktif sınıfları kaldır
        $('a#admin-btn').removeClass('active');

        // Şu an tıklanan <a> etiketi üzerine "profile-active" sınıfı ekle
        $(this).addClass('active');
    });

    // İkinci event handler: kategori seçimi değiştiğinde çalışacak
    $('#categorySelect').change(function () {
        const categoryId = $(this).val(); // Seçilen kategori ID'si.

        // Seçili kategori yoksa uyarı ver.
        if (!categoryId) {
            alert('Lütfen bir kategori seçin!');
            return;
        }

        // Alt kategori dropdown'u temizlenir.
        $('#subCategory').empty();

        // AJAX isteği yapılıyor.
        $.ajax({
            url: '/Product/getSubcategories', // Backend URL
            type: 'GET', // İstek türü
            dataType: 'json', // Dönen veri türü
            data: { categoryId: categoryId }, // Gönderilen parametre

            success: function (data) {
                console.log('Gelen Veri:', data); // Yanıtı kontrol etmek için.

                // Eğer veri boşsa uygun mesaj göster.
                if (!data || data.length === 0) {
                    $('#subCategory').append('<option disabled selected>Alt kategori bulunamadı</option>');
                    return;
                }

                // Alt kategorileri dropdown'a ekle.
                $.each(data, function (index, value) {
                    $('#subCategory').append(
                        `<option value="${value.value}">${value.text}</option>`
                    );
                });
            },

            error: function (xhr, status, error) {
                console.log('Hata:', error); // Hata mesajını logla.
                console.log('Durum:', status); // Durum bilgisi logla.
                console.log('Yanıt:', xhr.responseText); // Yanıtı logla.
                alert('Alt kategoriler yüklenirken bir hata oluştu.'); // Kullanıcıya uyarı göster.
            }
        });
    });

    // Üçüncü event handler: alt kategori seçimi değiştiğinde çalışacak
    $('#subCategory').change(function () {
        const selectedSubCategoryId = $(this).val(); // Seçilen alt kategori ID'si.

        // Seçilen alt kategori ID'sini konsola yazdır.
        console.log('Seçilen Alt Kategori ID:', selectedSubCategoryId);
        // Gizli input güncelleniyor.
        $('#selectedSubCategoryId').val(selectedSubCategoryId);

        // Burada seçilen alt kategori ID'si ile ilgili işlem yapabilirsiniz.
        // Örneğin, bir güncelleme isteği gönderilebilir.
    });
});
