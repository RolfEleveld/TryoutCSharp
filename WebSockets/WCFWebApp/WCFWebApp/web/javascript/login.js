function LoginPage() {
  
  var token;
  var activeuser;

  this.loadLoginPage = function () {
    loadPage();
  }

  var loadPage = function () {
    $.ajax({url: '/login.htm',
      dataType: 'html',
      beforeSend:function(xhr, settings) {
      },
      success: function(data, textStatus, xhr) {
                   $("#content")[0].innerHTML = xhr.responseText;
                   $("#login")[0].onclick = login;
               },
      error: function(xhr, textStatus) {
               if(xhr.status==400) {
               }
               if(xhr.status==401) {
                 loadLoginPage();
               }
             }
    });
  }

  this.getToken = function () {
    return token;
  }

  this.activeUser = function () {
    return activeuser;
  }

  var getTimeStamp = function () {
    var datePart, timePart;
    with(new Date())
    {
      datePart = getFullYear()+'-'+(getMonth()+1)+'-'+getDate();
      timePart = getHours()+':'+getMinutes()+':'+getSeconds();
    }
    return datePart +' '+timePart;
  }

  var make_base_auth = function (user, password) {
    var tok = user + ': ' + password;
    var hash = Base64.encode(tok);
    token = "Basic " + hash; 
    activeuser = user;
    return token
  }

  var setHeader = function (visible) {
    if (visible) {
      $("#logoninfo")[0].style.display = "inline";
      $("#logout")[0].onclick = logout;
    } else {
      $("#logoninfo")[0].style.display = "none";
    }
  }

  var login = function () {
    $.ajax({ type: 'POST', 
      url:'/login',
    data:JSON.stringify({"username":$("#username")[0].value}), 
    beforeSend:function(xhr, settings){
      xhr.setRequestHeader("Authorization", 
        make_base_auth($("#username")[0].value,$("#password")[0].value));
      xhr.setRequestHeader("Content-Type",'application/json');
    },
    success: function(data, textStatus, xhr) {
               $("#a_username")[0].textContent = $("#username")[0].value;
               $("#a_timestamp")[0].textContent = getTimeStamp();
               setHeader(true);

               loadAbout();
             },
    error: function(xhr, textStatus) {
             if(xhr.status==401) { 
               setHeader(false);
             }
           }
    });
  }

  var logout = function () {
    $.ajax({ type: 'POST', 
      url:'/logout',
    data:JSON.stringify({"username":activeuser}), 
    beforeSend:function(xhr, settings){
      xhr.setRequestHeader("Authorization", token);
      xhr.setRequestHeader("Content-Type",'application/json');
    },
    success: function(data, textStatus, xhr) {
               setHeader(false);
               $("#content").text(data);
               $('<a></a>')
      .attr('href',"#")
      .text('Login again')
      .appendTo($("#content"));
    $('#content a')[0].onclick = loadPage;
    token = undefined;
    activeuser = undefined;
             },
    error: function(data, textStatus, xhr) {
             if(xhr.status==401) {
               loadLoginPage();
             }
           }
    });

  }
}
