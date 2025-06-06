﻿//// Button elementlerini seç
//const signUpButton = document.getElementById('sign-up-button');
//const loginButton = document.getElementById('login-button');
//const loginButton1 = document.getElementById('login-button1');
//const loginAlert = document.querySelector('.alert alert-danger');
//const loginContainer = document.querySelector('.login-container .panel');// Panelin tüm kapsayıcısı



//// İşlevler
//function showSignUp() {
//    loginContainer.classList.add('show-sign-up');
//    loginContainer.classList.remove('show-login');
//    signUpButton.style.display = 'none';
//    loginButton.style.display = 'block';
//}

//function showLogin() {
//    loginContainer.classList.add('show-login');
//    loginContainer.classList.remove('show-sign-up');
//    signUpButton.style.display = 'block';
//    loginButton.style.display = 'none';
//}

//function showAlert() {
//    // Mesajı görünür yap
//    loginAlert.style.display = 'block';
//    setTimeout(function () {
//        loginAlert.style.display = 'none'
//    }, 2000);
//}



//loginButton.addEventListener('click', showLogin);
//signUpButton.addEventListener('click', showSignUp);
//loginButton1.addEventListener('click', showAlert);




function confirmDialog(title, message, confirmCallback, cancelCallback = null) {
    Swal.fire({
        title: title,
        text: message,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: '<i class="fa fa-check"></i> Onayla',
        cancelButtonText: '<i class="fa fa-times"></i> Vazgeç',
        reverseButtons: true // İstersen butonların sırasını değiştirebilirsin
    }).then((result) => {
        if (result.isConfirmed) {
            if (typeof confirmCallback === 'function') {
                confirmCallback();  // Onaylandığında yapılacak işlem
            }
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            if (typeof cancelCallback === 'function') {
                cancelCallback();  // Vazgeçildiğinde yapılacak işlem
            }
        }
    });
}









