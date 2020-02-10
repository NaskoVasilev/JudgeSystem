let currentPageClass = 'current-page';
window.onload = () => {
    $(".page-number")[0].classList.add(currentPageClass);
};

function InitializePaginationList(url, numberOfPagesUrl) {
    $(".page-number, #previous, #next").off();

    $(".page-number").on("click", (e) => {
        e.preventDefault();
        let page = Number.parseInt(e.target.textContent);
        getHtml(url, page);
    });


    $("#previous").on('click', (e) => {
        e.preventDefault();
        let currentPageNumber = Number.parseInt($(`.${currentPageClass}`)[0].innerText);

        $.get(numberOfPagesUrl)
            .done(lastPage => {
                if (currentPageNumber === 1) {
                    getHtml(url, Number.parseInt(lastPage));
                }
                else {
                    getHtml(url, --currentPageNumber);
                }
            })
            .fail(error => {
                showError(error.responseText);
            });
    });

    $("#next").on('click', (e) => {
        e.preventDefault();
        let currentPageNumber = Number.parseInt($(`.${currentPageClass}`)[0].innerText);

        $.get(numberOfPagesUrl)
            .done((lastPage) => {
                if (currentPageNumber === Number.parseInt(lastPage)) {
                    getHtml(url, 1);
                }
                else {
                    getHtml(url, ++currentPageNumber);
                }
            })
            .fail(error => {
                showError(error.responseText);
            });
    });
}

function getHtml(url, page) {
    let pageNumberPlaceholder = "{0}";
    let targetUrl = url.replace(pageNumberPlaceholder, page);
    $.get(targetUrl)
        .done(html => {
            document.getElementsByTagName("html")[0].innerHTML = html;
            InitializePaginationList(url, numberOfPagesUrl);
            let currentPageClass = "current-page";
            $(`.page-number:contains(${page})`)[0].classList.add(currentPageClass);
        })
        .fail(error => {
            showError(error.responseText);
        });
}

let itemsField = $('#entities-per-page');
if (itemsField) {
    itemsField.on('input', (e) => {
        console.log(itemsField.val());
    });
}