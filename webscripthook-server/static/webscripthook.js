function sendInput(cmd, arg, args, callback, errorCallback) {
  var stringified = JSON.stringify({ "Cmd": cmd, "Arg" : arg, "Args" : args });
  $.ajax({
    url: '/input',
    type: 'post',
    dataType: 'json',
    data: stringified,
    timeout: 2000,
    success: callback,
    error: errorCallback,
  });
}
