function StatesPage () {
  this.loadStatesPage = function (data) {
    content = undefined;
    $.each (data, function (index, value) {
      if (data[index]["id"]=="content") {
        content = data[index].innerHTML;
        $("#content")[0].innerHTML = content;
        $("#b_update")[0].onclick = updateStates;
        loadStatesTable();
      }
    });
    if (content == undefined) {alert("Failed to load page: Content missing"); return;} 
  }

  var loadStatesTable = function () {
    getdata('/states','json', function (data, textStatus, xhr) {
      var statesTable = $('#content table')[0];
      $.each(data.states, function (i,s) {

       var visitedControl = $('<input></input>')
            .attr('type','checkbox')
            .attr('checked',s.visited=="1")
       visitedControl[0]['modified'] = false;
       visitedControl[0].onclick = function () { this.modified = !this.modified; }
        
        $('<tr></tr>')
        .append($('<td>'+s.state+'</td>'))
        .append($('<td>'+s.capital+'</td>'))
        .append($('<td></td>').append(visitedControl))
        .appendTo(statesTable);

      });
    });
  }

  var updateStates = function () {
    var statesTable = $('#content table')[0];
    $.each(statesTable.rows, function (i,r) {
      if (i==0) {
        return true;
      }
      var checkBox = r.cells[2].children[0];
      if (checkBox.modified)  {
        $.ajax({ type: 'POST', 
          url:'/state',
          data:JSON.stringify({"id":i,"visited": checkBox.checked}), 
          beforeSend:function(xhr, settings){
            xhr.setRequestHeader("Authorization", auth.getToken());
            xhr.setRequestHeader("Content-Type",'application/json');
          },
          success: function(data, textStatus, xhr) {   
                   },
          error: function(xhr,error) {
                   if(xhr.status==401) {
                     auth.loadLoginPage();
                   }
                 }          
        });
      }
    });
  }
}
