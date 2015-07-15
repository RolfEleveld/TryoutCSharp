function AboutPage () {
  this.loadAboutPage = function (data) {
    content = undefined;
    $.each (data, function (index, value) {
      if (data[index]["id"]=="content") {
        content = data[index].innerHTML;
        $("#content")[0].innerHTML = content;
        $("#s_user").text(auth.activeUser());
      }
    });
    if (content == undefined) {alert("Failed to load page: Content missing"); return;} 
  }
}


