function radioPrev() {
  sendToGame("Radio", "0");
}

function radioNext() {
  sendToGame("Radio", "1");
}

function sendToGame(cmd, arg) {
  $.ajax({
    url: '/input',
    type: 'post',
    dataType: 'json',
    success: null,
    data: JSON.stringify({ "Cmd": cmd, "Arg" : arg }),
  });
}
