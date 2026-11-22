// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function reloadDictionary() {  

    $.ajax({
        type: 'GET',
        url: '/en/Latn/Home/Reload',
        contentType: false,
        processData: false,
        cache: false,
        success: function (data) {
            location.reload();
        },
        error: function () {
            alert("Something went wrong please contact admin.");
        }
    });
}

function addDallet(id) {
    var formData = new FormData();
    formData.append("id", id);    

    $.ajax({
        type: 'POST',
        url: '/en/Latn/Article/AddDallet',
        contentType: false,
        processData: false,
        cache: false,
        data: formData,
        success: function (data) {
            location.reload();
        },
        error: function () {
            alert("Something went wrong please contact admin.");
        }
    });
}

function toggleLetter(button) {
    switch (button.value) {
        case 'r': button.value = 'ṛ'; break;
        case 'ṛ': button.value = 'r'; break;
        /*case 'h': button.value = 'ḥ'; break;
        case 'ḥ': button.value = 'h'; break;*/

        case 'c': button.value = 'ċ'; break;
        case 'ċ': button.value = 'c'; break;

        case 'l': button.value = 'ḷ'; break;
        case 'ḷ': button.value = 'l'; break;

        case 's': button.value = 'ş'; break;
        case 'ş': button.value = 's'; break;

        case 'ṣ': button.value = 'ṥ'; break;
        case 'ṥ': button.value = 'ṣ'; break;

        case 'z': button.value = 'ž'; break;
        case 'ž': button.value = 'z'; break;

        case 'ẓ': button.value = 'ż'; break;
        case 'ż': button.value = 'ẓ'; break;

        case 'k': button.value = 'ḱ'; break;
        case 'ḱ': button.value = 'ḵ'; break;
        case 'ḵ': button.value = 'k'; break;

        case 't': button.value = 'ṫ'; break;
        case 'ṫ': button.value = 'ṯ'; break;
        case 'ṯ': button.value = 'ţ'; break;
        case 'ţ': button.value = 't'; break;

        case 'd': button.value = 'ḋ'; break;
        case 'ḋ': button.value = 'ḏ'; break;
        case 'ḏ': button.value = 'd'; break;

        case 'b': button.value = 'ḃ'; break;
        case 'ḃ': button.value = 'ḇ'; break;
        case 'ḇ': button.value = 'b'; break;

        case 'g': button.value = 'ġ'; break;
        case 'ġ': button.value = 'g'; break;

        case 'j': button.value = 'ɉ'; break;
        case 'ɉ': button.value = 'j'; break;

        case 'a': button.value = 'ᵃ'; break;
        case 'ᵃ': button.value = 'a'; break;
        case 'u': button.value = 'ᵘ'; break;
        case 'ᵘ': button.value = 'u'; break;
        case 'e': button.value = 'ᵉ'; break;
        case 'ᵉ': button.value = 'e'; break;
        case 'i': button.value = 'ⁱ'; break;
        case 'ⁱ': button.value = 'i'; break;
        case 'w': button.value = 'ʷ'; break;
        case 'ʷ': button.value = 'w'; break;
        default: return;
    }

    let inputs = button.parentElement.getElementsByTagName('input');

    let originalText = button.parentElement.getElementsByTagName('span')[0].innerText;
    let oldValue = originalText[Array.from(inputs).indexOf(button)];

    button.style.color = button.value == oldValue ? "black" : "blue";

    let text = '';

    for (i = 0; i < inputs.length; i++) {
        text += inputs[i].value
    }


    var formData = new FormData();
    formData.append("id", button.parentElement.getAttribute("id"));
    formData.append("Text", text);

    $.ajax({
        type: 'POST',
        url: '/en/Latn/Article/ToggleEdit',
        contentType: false,
        processData: false,
        cache: false,
        data: formData,
        success: function (data) {
        },
        error: function () {
            alert("Something went wrong please contact admin.");
        }
    });
}