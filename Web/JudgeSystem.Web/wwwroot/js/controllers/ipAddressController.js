//Delete ip address
let clickedDeleteButton;

$("#delete-confirm-btn").on('click', (e) => {

    if (clickedDeleteButton) {
        let ipAddressId = clickedDeleteButton.dataset.id;

        $.post('/Administration/AllowedIpAddress/Delete', { id: ipAddressId })
            .done((response) => {
                $(clickedDeleteButton.parentElement.parentElement).hide();
                showInfo(response);
            })
            .fail((error) => {
                showError(error.responseText);
            });
    }
});

$(".ipAddressDeleteBtn").on('click', (e) => {
    clickedDeleteButton = $(e.target)[0];
});