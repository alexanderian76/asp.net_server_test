// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getPhoneById() {
    console.log($("#input_id").val())
    fetch("Home/GetPhoneById?id=" + $("#input_id").val()).then(res => res.text().then(txt => $("#title").html(txt)))
}




function getTestPdf() {
    let data = []
    
    fetch("Home/GetPdfFromHtml").then(res => {
        const reader = res.body.getReader()
        function readStream({ done, value }) {
            if (done) {
                saveByteArray("name.pdf", data)
                return;
            }
            data.push(value)
            return reader.read().then(readStream);
        }
        reader.read().then(d => {
            readStream(d)
        })
        
    });
}

function inputChanged(e) {
    console.log(e)
    console.log($("#input_id"))
}
function createPhoneWithRandomPrice() {
    fetch("Home/CreateRandomPhone?title=" + $("#input_id").val(), {method: "POST"}).then(res => res.text().then(txt => $("#title").html(txt)))
}


function saveByteArray(reportName, byte) {
    var blob = new Blob(byte, { type: "application/pdf" });
    var link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    var fileName = reportName;
    link.download = fileName;
    link.click();
};

$(() => {
    $("#input_id").on('input', inputChanged)
})