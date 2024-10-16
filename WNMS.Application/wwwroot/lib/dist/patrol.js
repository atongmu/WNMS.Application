$(function () {
    //自定弹窗关闭事件
    $('.potrolPopup .potrolPopup_close').click(function () {
        $(this).parent().parent('.potrolPopup').hide()
        $(this).parent().siblings('.potrolPopup_body').children('.potrolPopup').hide()
    })
    $('.potrolPopup .btn-default').click(function () {
        $(this).parent().parent().hide()
        $(this).parent().siblings('.potrolPopup_body').children('.potrolPopup').hide()
    })
    $('.potrolPopup .btn-primaryTc').click(function () {
        $(this).parent().parent().hide()
        $(this).parent().siblings('.potrolPopup_body').children('.potrolPopup').hide()
    })
    //自定弹窗2关闭事件
    $('.potrolMap_popup .potrolPopup_close').click(function () {
        $(this).parent().parent('.potrolMap_popup').hide()
    })

    $('.selse_input').click(function () {
        $(this).siblings('.DMAName_div').show()
        return false
    })
    $('.DMAName_div').click(function () {
        $(this).show()
        return false
    })
    $('.potrolPopup_body').click(function () {
        $('.DMAName_div').hide()
        
    })
})





















