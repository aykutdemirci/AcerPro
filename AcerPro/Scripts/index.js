$(document).ready(async function () {

    const applications = [];
    $('#tbl_applications tbody tr').each(function (index, tr) {
        const data = {
            id: $(tr).attr('id'),
            url: $(tr).find('td.app-url').text(),
            interval: parseInt($(tr).find('td.interval').text())
        }

        applications.push(data);
    });

    if (applications.length == 0) return;

    for (var i = 0; i < applications.length; i++) {

        const app = applications[i];
        app.interval = app.interval * 1000;

        intervalFunction(app);
    }
});

function intervalFunction(app = { id: 0, url: '', interval: 0 }) {
    setTimeout(async function () {
        const statusCol = $(`#tbl_applications tbody tr[id="${app.id}"] td.status`);
        statusCol.text('checking');
        const response = await checkApplication(app);
        statusCol.text(response);
    }, app.interval);
}

async function checkApplication(app = { id: 0, url: '', interval: 0 }) {
    return new Promise(async function (resolve, reject) {
        $.ajax({
            type: 'POST',
            url: '/Home/CheckApplication',
            data: { url: app.url },
            error: function (jqXHR, textStatus) {
                resolve(textStatus)
            },
            success: function (response) {
                resolve(response);
            },
            complete: function () {
                intervalFunction(app);
            }
        });
    });
}