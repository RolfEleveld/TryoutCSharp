var auth;
var aboutPage ;
var statesPage ;
window.onload = function () { 
  auth = new LoginPage();
  auth.loadLoginPage();
  $('#a_states')[0].onclick = loadStates;
  $('#a_about')[0].onclick = loadAbout;
}	

function getdata(path, type, successhandler) {
  $.ajax({url: path,	
    dataType: type,
    type: "GET",
  beforeSend: function(xhr, settings) {
    xhr.setRequestHeader("Authorization",auth.getToken());
  },
    success: successhandler,
    error: function(xhr,error) {
      if(xhr.status==401) {
        auth.loadLoginPage();
      }
    }
  });
}

function loadAbout () {
  aboutPage = new AboutPage();
  getdata('/about.htm','html', function (data, textStatus, xhr) {
    aboutPage.loadAboutPage($(data));
  });
}

function loadStates () {
  statesPage = new StatesPage();
  getdata('/states.htm','html', function (data, textStatus, xhr) {
    statesPage.loadStatesPage($(data));
  });
}
