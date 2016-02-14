function getTypeDropdown() {
  var data = {
    'string': 'string',
    'int': 'int',
    'float': 'float',
    'bool': 'bool',
  }
  var s = $('<select class="form-control" />');
  for(var val in data) {
    $('<option />', {value: val, text: data[val]}).appendTo(s);
  }
  return s.prop('outerHTML');
}

function addArgsClick() {
  var id = ($('#argsDivs div').length).toString();
  $('#argsDivs').append('<div class="form-group" id="argDiv' + id + '">' + getTypeDropdown() + '<input type="text" class="form-control" autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" placeholder="Args[' + id + ']..." id="args' + id + '"></div>');
}

function removeArgsClick() {
  if ($('#argsDivs').length == 0) {
    console.log("No more textbox to remove");
    return false;
  }
  $("#argsDivs div:last").remove();
}

function sendInputClick() {
  $("#sendResultTitle").text("Not sent");
  $("#sendResultBody").text("");
  var cmd = $("#cmd").val();
  var arg = $("#arg").val();
  if (!cmd) {
    $("#sendResultBody").text("Empty cmd");
    return;
  };
  var args = [];
  var argDivs = $("#argsDivs").children("div");
  for (var i = 0; i < argDivs.length; i++) {
    var aType = $(argDivs[i]).find("select option:selected").text();
    var aData = $(argDivs[i]).find("input").val();
    if (aType == "int") {
      aData = Number(aData);
    } else if (aType == "float") {
      aData = "{float}" + aData;
    } else if (aType == "bool") {
      aData = aData.toLowerCase() == "true" ? true : false;
    }
    args.push(aData);
  }
  sendInput(cmd, arg, args, function(data) {
    $("#sendResultTitle").text("Sent");
    $("#sendResultBody").text(data);
  });
}

function sendInput(cmd, arg, args, callback) {
  var stringified = JSON.stringify({ "Cmd": cmd, "Arg" : arg, "Args" : args }, null, 2);
  $.ajax({
    url: '/input',
    type: 'post',
    dataType: 'json',
    success: null,
    data: stringified,
  }).done(callback);
}
