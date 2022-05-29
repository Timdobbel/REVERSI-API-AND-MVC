"use strict";

function _createForOfIteratorHelper(o, allowArrayLike) { var it = typeof Symbol !== "undefined" && o[Symbol.iterator] || o["@@iterator"]; if (!it) { if (Array.isArray(o) || (it = _unsupportedIterableToArray(o)) || allowArrayLike && o && typeof o.length === "number") { if (it) o = it; var i = 0; var F = function F() {}; return { s: F, n: function n() { if (i >= o.length) return { done: true }; return { done: false, value: o[i++] }; }, e: function e(_e) { throw _e; }, f: F }; } throw new TypeError("Invalid attempt to iterate non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); } var normalCompletion = true, didErr = false, err; return { s: function s() { it = it.call(o); }, n: function n() { var step = it.next(); normalCompletion = step.done; return step; }, e: function e(_e2) { didErr = true; err = _e2; }, f: function f() { try { if (!normalCompletion && it["return"] != null) it["return"](); } finally { if (didErr) throw err; } } }; }

function _unsupportedIterableToArray(o, minLen) { if (!o) return; if (typeof o === "string") return _arrayLikeToArray(o, minLen); var n = Object.prototype.toString.call(o).slice(8, -1); if (n === "Object" && o.constructor) n = o.constructor.name; if (n === "Map" || n === "Set") return Array.from(o); if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen); }

function _arrayLikeToArray(arr, len) { if (len == null || len > arr.length) len = arr.length; for (var i = 0, arr2 = new Array(len); i < len; i++) { arr2[i] = arr[i]; } return arr2; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); Object.defineProperty(Constructor, "prototype", { writable: false }); return Constructor; }

var Game = function (url) {
  var stateMap = {
    gameState: null,
    prev: null,
    gameId: "02af7930-6faf-45a8-bf90-961cc5e40beb",
    playerId: "",
    mvcURL: ""
  };
  var polling;
  var _configMap = {};

  var privateInit = function privateInit(gameToken, playerId, mvcURL) {
    stateMap.gameId = gameToken;
    stateMap.playerId = playerId;
    stateMap.mvcURL = mvcURL;
    Game.Data.init("".concat(stateMap.mvcURL), "production");
    Game.Model.init();
    Game.Template.init();
    Game.API.init();
    Game.Stats.init();
    Game.Reversi.init();
    var leaveButton = document.getElementById('leaveButton');

    leaveButton.onclick = function (event) {
      if (confirm('Weet je zeker dat je het spel wilt verlaten?') == true) {
        $.get("".concat(stateMap.mvcURL, "/Spel/Verlaten/").concat(stateMap.gameId, "?spelerToken=").concat(stateMap.playerId)).then(function (r) {
          return window.location = "/";
        });
      }
    };

    _getGameState();

    polling = setInterval(_getGameState, 2000);
  };

  var placeFiche = function placeFiche(x, y) {
    var e = document.getElementById('beurt');
    e.innerText = 'Laden...'; // console.log('aan de beurt: ' + aanDeBeurt());

    if (!aanDeBeurt()) return;
    Game.Data.put("/api/spel/".concat(stateMap.gameId, "/zet?token=").concat(stateMap.playerId, "&kolom=").concat(x, "&rij=").concat(y));
    return '';
  };

  function aanDeBeurt() {
    var token = stateMap.gameState.aanDeBeurt === 1 ? stateMap.gameState.speler1Token : stateMap.gameState.speler2Token;
    return token === stateMap.playerId;
  }

  var updateBeurtText = function updateBeurtText() {
    var e = document.getElementById('beurt');
    var beurt = aanDeBeurt();

    if (beurt) {
      $(e).removeClass('beurt--jouw');
      $(e).addClass('beurt--mijn');
    } else {
      $(e).removeClass('beurt--mijn');
      $(e).addClass('beurt--jouw');
    }

    e.innerText = beurt ? 'Je bent aan de beurt' : 'De ander is aan de beurt';
  };

  var updateWonOrLost = function updateWonOrLost() {
    var p1 = stateMap.gameState.bord.map(function (v) {
      return v.filter(function (l) {
        return l === 1;
      }).length;
    }).reduce(function (i1, i2) {
      return i1 + i2;
    });
    var p2 = stateMap.gameState.bord.map(function (v) {
      return v.filter(function (l) {
        return l === 2;
      }).length;
    }).reduce(function (i1, i2) {
      return i1 + i2;
    });
    var isP1 = stateMap.gameState.speler1Token === stateMap.playerId;
    var e = document.getElementById('beurt');

    if (p1 > p2) {
      e.innerHTML = isP1 ? "🔥🎊🔥🙌🎈🍻 Gewonnen! 🔥🎊🔥🙌🎈🍻" : "Je hebt verloren 😢😔";
    } else if (p2 > p1) {
      e.innerHTML = isP1 ? "Je hebt verloren 😢😔" : "🔥🎊🔥🙌🎈🍻 Gewonnen! 🔥🎊🔥🙌🎈🍻";
    } else {
      e.innerHTML = "Gelijk spel";
    }

    var leaveButton = document.getElementById('leaveButton');
    leaveButton.innerText = "Spel Verlaten ✅🏁";
    leaveButton.removeAttribute("onclick");

    leaveButton.onclick = function (event) {
      $.get("".concat(stateMap.mvcURL, "/Spel/Finish/").concat(stateMap.gameId)).then(function (r) {
        return window.location = "/";
      });
    };
  };

  var updateScore = function updateScore() {
    var p1 = stateMap.gameState.bord.map(function (v) {
      return v.filter(function (l) {
        return l === 1;
      }).length;
    }).reduce(function (i1, i2) {
      return i1 + i2;
    });
    var p2 = stateMap.gameState.bord.map(function (v) {
      return v.filter(function (l) {
        return l === 2;
      }).length;
    }).reduce(function (i1, i2) {
      return i1 + i2;
    });
    var e = document.getElementById("score");

    if ("".concat(p1, " - ").concat(p2) !== "2 - 2") {
      e.innerText = "".concat(p1, " - ").concat(p2);
    }
  };

  var _getGameState = function _getGameState() {
    var first = stateMap.gameState === null;
    Game.Model.getGameState(stateMap.gameId).then(function (response) {
      Game.Model.getAfgelopen(stateMap.gameId).then(function (afgelopen) {
        if (afgelopen) {
          updateWonOrLost();
          clearInterval(polling);
        }
      })["catch"](function (err) {
        return console.error(err);
      });
      stateMap.gameState = response;
      updateBeurtText();

      if (first) {
        Game.Reversi.renderBoard(stateMap.gameState.bord);
      } else {
        var changed = false;

        for (var x = 0; x < 8; x++) {
          for (var y = 0; y < 8; y++) {
            var prev = stateMap.prev.bord[y][x];
            var now = stateMap.gameState.bord[y][x];

            if (prev !== now) {
              changed = true;
              Game.Reversi.showFiche(x, y, now);
            }
          }
        }

        if (changed) {
          updateScore();
          Game.Stats.addData(stateMap.gameState);
        }
      }

      stateMap.prev = stateMap.gameState;
    })["catch"](function (err) {
      return console.error(err);
    });
  };

  return {
    init: privateInit,
    placeFiche: placeFiche
  };
}('/api/url');

var FeedbackWidget = /*#__PURE__*/function () {
  function FeedbackWidget(elementId) {
    _classCallCheck(this, FeedbackWidget);

    this._elementId = elementId;
  }

  _createClass(FeedbackWidget, [{
    key: "elementId",
    get: function get() {
      //getter, set keyword voor setter methode
      return this._elementId;
    }
  }, {
    key: "show",
    value: function show(message, type) {
      var x = $("#" + this._elementId);
      var messageElement = $("#alert-message");
      $(x).fadeIn("slow");
      messageElement.text(message);
      messageElement.addClass('yeet');

      if (type === "danger") {
        $(x).addClass('feedback-widget--state-danger');
        $(x).removeClass('feedback-widget--state-succes');
      } else if (type === "success") {
        $(x).addClass('feedback-widget--state-succes');
        $(x).removeClass('feedback-widget--state-danger');
      }

      var msg = {
        message: message,
        type: type
      };
      this.log(msg);
    }
  }, {
    key: "hide",
    value: function hide() {
      var x = document.getElementById(this._elementId);
      $(x).fadeOut("slow");
    }
  }, {
    key: "log",
    value: function log(message) {
      {
        //Get local storage item by key
        var arr = JSON.parse(localStorage.getItem('feedback_widget')); //Set initial local storage

        if (arr == null) {
          localStorage.setItem('feedback_widget', JSON.stringify([message]));
        } else {
          //Push new log into array
          arr.push(message); //Remove first entry

          if (arr.length > 10) {
            arr.shift();
          } //Remove array


          this.removelog(); //Add full array

          localStorage.setItem('feedback_widget', JSON.stringify(arr));
        }
      }
    }
  }, {
    key: "removelog",
    value: function removelog() {
      localStorage.removeItem('feedback_widget');
    }
  }, {
    key: "history",
    value: function history() {
      var _JSON$parse;

      var arr = (_JSON$parse = JSON.parse(localStorage.getItem("feedback_widget"))) !== null && _JSON$parse !== void 0 ? _JSON$parse : [];
      var messages = arr.map(function (v) {
        return "".concat(v.type, " - ").concat(v.message);
      }).join("\n");
      console.log(messages);
    }
  }]);

  return FeedbackWidget;
}();

Game.API = function () {
  function init() {
    console.log('from game.api.init!');
  }

  return {
    init: init
  };
}();

Game.Data = function () {
  var stateMap = {
    environment: 'development' // environment: 'production'

  };
  var configMap = {
    url: "",
    apiKey: "<plaats hier je apikey>",
    mock: [{
      url: "/api/spel/beurt",
      data: 0
    }]
  };
  1;

  var privateInit = function privateInit(url, environment) {
    configMap.url = url;

    if (environment !== 'production' && environment !== 'development') {
      throw new Error('environment must be production or development');
    }

    stateMap.environment = environment;
    console.log(stateMap.environment + ' environment');
    console.log('from game.data.init!');
  };

  var get = function get(url) {
    if (stateMap.environment === 'development') {
      return getMockData(url);
    } else if (stateMap.environment === 'production') {
      // console.log("fetching: " + configMap.url + url);
      return new Promise(function (resolve, reject) {
        $.ajax({
          url: configMap.url + url,
          type: 'GET',
          success: function success(r) {
            resolve(r);
          },
          error: function error(err) {
            reject(err);
          }
        });
      });
    }
  };

  var put = function put(url) {
    if (stateMap.environment === "development") return getMockData(url); // console.log("DoeZet Url;" + configMap.url + url);

    return new Promise(function (resolve) {
      $.ajax({
        url: configMap.url + url,
        type: 'PUT',
        success: function success(r) {
          resolve(r);
        }
      });
    });
  };

  var getMockData = function getMockData(url) {
    var mockData = configMap.mock.find(function (item) {
      return item.url === url;
    }).data;
    return new Promise(function (resolve, reject) {
      resolve(mockData);
    });
  };

  return {
    init: privateInit,
    get: get,
    put: put
  };
}();

Game.Model = function () {
  var configMap = {};

  var _getGameState = function _getGameState(token) {
    return new Promise(function (resolve, reject) {
      // console.log(`getting game for: speltoken: ${token}`);
      Game.Data.get("/Spel/GetSpel/".concat(token !== null && token !== void 0 ? token : "")).then(function (res) {
        var aanDeBeurt = res.aanDeBeurt;
        convertBoard(res);

        if (aanDeBeurt === 0 || aanDeBeurt === 1 || aanDeBeurt === 2) {
          resolve(res);
        } else {
          console.error("Game.Model.getGameState: Error fetching gamestate");
          reject("Invalid gamestate or failed request.");
        }
      })["catch"](function (err) {
        return reject(err);
      });
    });
  };

  var _getAfgelopen = function _getAfgelopen(token) {
    return new Promise(function (resolve, reject) {
      Game.Data.get("/Spel/Afgelopen/".concat(token !== null && token !== void 0 ? token : "")).then(function (res) {
        resolve(res);
      })["catch"](function (err) {
        return reject(err);
      });
    });
  };

  function convertBoard(gameState) {
    var board = gameState.bord;
    var newBoard = [];

    for (var i = 0; i < 8; i++) {
      newBoard.push([0, 0, 0, 0, 0, 0, 0, 0]);
    }

    for (var pos in board) {
      var color = board[pos];
      var split = pos.split(",");
      newBoard[split[0]][split[1]] = color;
    }

    gameState.bord = newBoard;
  }

  function _init() {
    console.log('from game.model.init!');
  }

  return {
    init: _init,
    getGameState: _getGameState,
    getAfgelopen: _getAfgelopen
  };
}();

Game.Reversi = function () {
  var configMap = {
    boardTemplate: "board.body"
  };

  function _init() {
    console.log('from game.reversi.init!');
  }

  function renderBoard(newBoard) {
    var board = document.querySelector(".board");
    board.innerHTML = Game.Template.parseTemplate(configMap.boardTemplate, {
      board: newBoard
    });

    var _loop = function _loop(y) {
      var _loop2 = function _loop2(x) {
        var place = document.querySelector(".board__row__element--y-".concat(y, ".board__row__element--x-").concat(x));
        place.addEventListener("click", function (evt) {
          if (place.children.length > 0) return;
          Game.placeFiche(x, y);
        });
      };

      for (var x = 0; x < 8; x++) {
        _loop2(x);
      }
    };

    for (var y = 0; y < 8; y++) {
      _loop(y);
    }
  }

  function showFiche(x, y, player) {
    var place = document.querySelector(".board__row__element--y-".concat(y, ".board__row__element--x-").concat(x));
    var fiche = document.createElement("div");
    fiche.classList.add("fiche");
    fiche.classList.add("fiche--".concat(player));
    place.innerHTML = "";
    place.append(fiche);
  }

  return {
    init: _init,
    renderBoard: renderBoard,
    showFiche: showFiche
  };
}();

Game.Stats = function () {
  var myChart;
  var p1Fiches = [];
  var p2Fiches = [];

  function _init() {
    console.log('from game.stats.init!');
    update();
  }

  function update() {
    var _myChart;

    var ctx = $('#stats');
    (_myChart = myChart) === null || _myChart === void 0 ? void 0 : _myChart.destroy();
    myChart = new Chart(ctx, {
      type: 'line',
      data: {
        labels: p1Fiches.map(function (value, index) {
          return index + 1;
        }),
        datasets: [{
          label: 'Speler 1',
          data: p1Fiches,
          fill: false,
          borderColor: 'rgb(179, 91, 91)',
          tension: 0.1
        }, {
          label: 'Speler 2',
          data: p2Fiches,
          fill: false,
          borderColor: 'rgb(91, 179, 94)',
          tension: 0.1
        }]
      },
      options: {
        scales: {
          y: {
            beginAtZero: true
          }
        }
      }
    });
  }

  function addData(data) {
    var p1 = data.bord.map(function (v) {
      return v.filter(function (l) {
        return l === 1;
      }).length;
    }).reduce(function (i1, i2) {
      return i1 + i2;
    });
    var p2 = data.bord.map(function (v) {
      return v.filter(function (l) {
        return l === 2;
      }).length;
    }).reduce(function (i1, i2) {
      return i1 + i2;
    });
    p1Fiches.push(p1);
    p2Fiches.push(p2);
    update();
  }

  return {
    init: _init,
    addData: addData
  };
}();

Game.Template = function () {
  var configMap = {};

  function getTemplate(templateName) {
    var templates = spa_templates.templates;

    var _iterator = _createForOfIteratorHelper(templateName.split(".")),
        _step;

    try {
      for (_iterator.s(); !(_step = _iterator.n()).done;) {
        var t = _step.value;
        templates = templates[t];
      }
    } catch (err) {
      _iterator.e(err);
    } finally {
      _iterator.f();
    }

    return templates;
  }

  function parseTemplate(templateName, data) {
    return getTemplate(templateName)(data);
  }

  function _init() {
    Handlebars.registerHelper('ifeq', function (a, b, options) {
      if (a === b) {
        return options.fn(this);
      }

      return options.inverse(this);
    });
    Handlebars.registerHelper('ifnoteq', function (a, b, options) {
      if (a !== b) {
        return options.fn(this);
      }

      return options.inverse(this);
    });
  }

  return {
    init: _init,
    getTemplate: getTemplate,
    parseTemplate: parseTemplate
  };
}();