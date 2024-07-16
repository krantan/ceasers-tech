// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
  const dotoast = (message) => {
    $('#toast').show().find('.toast-body').text(message);
  };

  const json_form_submit = (target) => {
    const raw = target.serializeArray();
    const data = {};

    $.map(raw, (n) => {
      data[n['name']] = n['value'];
    });

    $.ajax({
      url: target.attr("action"),
      type: "POST",
      contentType : 'application/json',
      data: JSON.stringify(data),
      success: function(rs) {
        var path = target.data('finish');
        if (path) {
          return window.location = path
        }
        window.location.reload();
      },
      complete: function (rs) {
        const data = JSON.parse(rs.responseText);
        if (!data.success && data.message) {
          dotoast(data.message);
        }
        
        return false;
      }
    })
  };

  $(".json-form").submit((e) => {
    let target = $(e.target);

    try {
      if (target.is(".form-confirm")) {
        if (!confirm(target.data('confirm'))) {
          return false;
        }
      }

      json_form_submit(target);
      
      return false;
    } catch (e) {
      console.log(e)
      return false;
    }
  });

  $("#toast button.btn-close").click((e) => {
    $('#toast').hide();
  });
});
