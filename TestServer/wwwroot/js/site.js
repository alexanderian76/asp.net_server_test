// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getPhoneById() {
    console.log($("#input_id").val())
    fetch("Home/GetPhoneById?id=" + $("#input_id").val()).then(res => res.text().then(txt => $("#title").html(txt)))
}
function getTestPdf() {
    fetch("Home/GetTestPdf");
}

function inputChanged(e) {
    console.log(e)
    console.log($("#input_id"))
}
function createPhoneWithRandomPrice() {
    fetch("Home/CreateRandomPhone?title=" + $("#input_id").val(), {method: "POST"}).then(res => res.text().then(txt => $("#title").html(txt)))
}

$(() => {
    $("#input_id").on('input', inputChanged)
})