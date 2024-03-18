////function MauXeSwitch() {
////    if (document.getElementById("tab-content-1").style.display == 'none') {
////        document.getElementById("tab-content-1").style.display = 'block';
////    }
////    else {
////        document.getElementById("tab-content-1").style.display = 'none';
////    }
////}


function ChangeImage(UploadHinh1, previewImg) {
    if (UploadHinh1.files && UploadHinh1.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImg).attr('src', e.target.result);
        }
        reader.readAsDataURL(UploadHinh1.files[0]);
    }
}

//function getVal() {
//    document.getElementById("ChiTietHd.KhuyenMai").value = document.getElementById("KhuyenMai").value
//    document.getElementById("ChiTietHd.GiaBan").value = document.getElementById("GiaBan").value
//}