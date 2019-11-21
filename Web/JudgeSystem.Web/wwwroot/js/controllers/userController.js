const initialRows = $('.data-row');

$('#username').on('keyup', (input) => {
    clearOtherFields(0);
    filter(0, input.target.value);
});

$('#first-name').on('keyup', (input) => {
    clearOtherFields(1);
    filter(1, input.target.value);
});

$('#last-name').on('keyup', (input) => {
    clearOtherFields(2);
    filter(2, input.target.value);
});

$('#email').on('keyup', (input) => {
    clearOtherFields(3);
    filter(3, input.target.value);
});

function filter(index, filter) {
    filter = filter.toLowerCase();
    if (!filter) {
        initialRows.show();
    }

    let rowsToShow = [];
    let rowsToHide = [];

    for (var i = 0; i < initialRows.length; i++) {
        let currentRow = initialRows[i];
        let tds = $(currentRow).find('td');
        let value = tds[index].innerText.toLowerCase();

        if (value.startsWith(filter)) {
            rowsToShow.push(currentRow);
        }
        else {
            rowsToHide.push(currentRow);
        }
    }

    $(rowsToShow).show();
    $(rowsToHide).hide();
}

function clearOtherFields(index) {
    let inputs = $('.inputs > th > input');
    for (let i = 0; i < inputs.length; i++) {
        if (i != index) {
            $(inputs[i]).val('');
        }
    }
}