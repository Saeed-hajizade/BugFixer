document.addEventListener("DOMContentLoaded", function () {
    hideCustomizer();
});

const hideCustomizer = () => {
    const customizer = document.getElementById("template-customizer");
    if (customizer != null) {
        customizer.style.display = "none";
    } else {
        setTimeout(hideCustomizer, 1);
    }
}

function FillPageId(id) {
    $("#Page").val(id);
    $("#filter-search").submit();
}
jQuery(document).ready(function () {
    "use strict";




    $('.gallery-main').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        fade: true,
        asNavFor: '.left-slider-image'
    });

    $('.left-slider-image').slick({
        slidesToShow: 4,
        slidesToScroll: 1,
        asNavFor: '.gallery-main',
        dots: false,
        focusOnSelect: true,
        vertical: true,
        center: true,
        responsive: [{
            breakpoint: 1400,
            settings: {
                slidesToShow: 4,
                vertical: true,
            }
        },
        {
            breakpoint: 1200,
            settings: {
                slidesToShow: 4,
                vertical: true,
            }
        },
        {
            breakpoint: 992,
            settings: {
                slidesToShow: 4,
                vertical: false,
            }
        },
        {
            breakpoint: 768,
            settings: {
                slidesToShow: 3,
                vertical: false,
            }
        }, {
            breakpoint: 576,
            settings: {
                slidesToShow: 3,
                vertical: false,
            }
        }, {
            breakpoint: 430,
            settings: {
                slidesToShow: 3,
                vertical: false,
            }
        },
        ]
    });


    new WOW().init();
});


function format(input) {
    var nStr = input.value + '';
    nStr = nStr.replace(/\,/g, "");
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    input.value = x1 + x2;
}

let inputPrice = document.getElementById("digitPrice");
inputPrice?.addEventListener("keyup", () => format(inputPrice));

let inputPrices = document.getElementsByClassName("digit-price");

if (inputPrices) {
    Array.from(inputPrices).forEach(input => {
        input.addEventListener("keyup", () => format(inputPrice));
    })
}


//#region Change Image 

$("[ImageInput]").change(function () {
    var x = $(this).attr("ImageInput");
    if (this.files && this.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("[ImageFile=" + x + "]").attr('src', e.target.result);
        };
        reader.readAsDataURL(this.files[0]);
    }
});

// #endregion

//#region Back Button

function BackPage() {
    history.back();
}

window.addEventListener('load', () => {
    setTimeout(() => {
        const backToTopBtn = document.querySelector('#back-to-top-btn');
        if (backToTopBtn)
            backToTopBtn.style.display = 'block';
    }, 5000); // 5000 milliseconds = 5 seconds
});
// Show the button when the user scrolls down a certain distance
window.addEventListener('scroll', () => {
    const backToTopBtn = document.querySelector('#back-to-top-btn');
    if (backToTopBtn)
        backToTopBtn.style.visibility = 'visible';
});

//#endregion
