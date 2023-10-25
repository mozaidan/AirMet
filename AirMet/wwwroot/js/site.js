// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


/* Set the width of the side navigation to 250px */
function openNav() {
    var sideNav = document.getElementById("mySidenav");
    if (sideNav) {
        sideNav.style.width = "250px";
    } else {
        console.error("Element with ID 'mySidenav' not found.");
    }
}

function closeNav() {
    var sideNav = document.getElementById("mySidenav");
    if (sideNav) {
        sideNav.style.width = "0";
    } else {
        console.error("Element with ID 'mySidenav' not found.");
    }
}


document.addEventListener("DOMContentLoaded", function () {
    // Function to add new image input
    function addImageInput() {
        const imageInput = document.createElement("input");
        imageInput.type = "file";
        imageInput.name = "Property.Files";
        imageInput.className = "form-control-file mb-2";
        imageInput.addEventListener("change", function () {
            const file = this.files[0];
            const reader = new FileReader();
            reader.onload = function (e) {
                const imgDiv = document.createElement("div");
                imgDiv.className = "img-div";

                const img = document.createElement("img");
                img.src = e.target.result;
                img.width = 100;

                const deleteBtn = document.createElement("button");
                deleteBtn.innerHTML = "x";
                deleteBtn.className = "delete-btn";
                deleteBtn.addEventListener("click", function () {
                    imgDiv.remove();
                    imageInput.value = ""; // Clear the input value
                });

                imgDiv.appendChild(img);
                imgDiv.appendChild(deleteBtn);
                document.getElementById("preview-container").appendChild(imgDiv);
            };
            reader.readAsDataURL(file);
        });
        document.getElementById("image-container").appendChild(imageInput);
    }

    // Add the first image input
    addImageInput();

    // Add more image inputs when the button is clicked
    document.getElementById("add-image").addEventListener("click", function () {
        addImageInput();
    });

});

// Optional: Add some JavaScript to highlight the active thumbnail
var thumbnails = document.querySelectorAll('.img-thumbnail');
var carouselInner = document.querySelector('.carousel-inner');

carouselInner.addEventListener('slide.bs.carousel', function (event) {
    // Remove 'active-thumbnail' class from all thumbnails
    thumbnails.forEach(function (thumbnail) {
        thumbnail.classList.remove('active-thumbnail');
    });

    // Add 'active-thumbnail' class to the new active thumbnail
    thumbnails[event.to].classList.add('active-thumbnail');
});

const stars = document.querySelectorAll('.star');
const rating = document.getElementById('selected-rating');

stars.forEach(star => {
    star.addEventListener('click', function () {
        const ratingValue = this.getAttribute('data-star');
        rating.textContent = ratingValue;

        stars.forEach(s => {
            s.classList.remove('selected');
        });

        for (let i = 0; i < ratingValue; i++) {
            stars[i].classList.add('selected');
        }
    });
});


