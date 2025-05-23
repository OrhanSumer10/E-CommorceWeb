function showToast(message, type = 'success') {
    const toastBox = document.getElementById('toastBox');
    if (!toastBox) return;

    toastBox.style.display = 'flex';

    toastBox.classList.add('toast'); // önce tüm classları sıfırla
    if (type === 'error') {
        toastBox.classList.add('error');
    } else if (type === 'invalid') {
        toastBox.classList.add('invalid');
    }

    let icon = '';
    if (type === 'success') {
        icon = '<i class="fa-solid fa-circle-check"></i>';
    } else if (type === 'error') {
        icon = '<i class="fa-solid fa-circle-xmark"></i>';
    } else if (type === 'invalid') {
        icon = '<i class="fa-solid fa-triangle-exclamation"></i>';
    }

    toastBox.innerHTML = `${icon}<span>${message}</span>`;
    

    setTimeout(() => {
        toastBox.style.display = 'none';
    }, 5000);
}

// Form gönderilmeden önce toast ve localStorage'a yaz
function showToastAndSave(message, type = 'success') {
    showToast(message, type);
    localStorage.setItem('toastMessage', message);
    localStorage.setItem('toastType', type);
}

// Sayfa yüklendiğinde
window.addEventListener('load', () => {
    const msg = localStorage.getItem('toastMessage');
    const type = localStorage.getItem('toastType');

    if (msg) {
        showToast(msg, type);
        // Mesajı gösterdikten sonra temizle, ki tekrarlanmasın
        localStorage.removeItem('toastMessage');
        localStorage.removeItem('toastType');
    }
});