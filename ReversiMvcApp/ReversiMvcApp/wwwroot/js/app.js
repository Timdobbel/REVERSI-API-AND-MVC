"use strict";function _createForOfIteratorHelper(e,t){var n,r="undefined"!=typeof Symbol&&e[Symbol.iterator]||e["@@iterator"];if(!r){if(Array.isArray(e)||(r=_unsupportedIterableToArray(e))||t&&e&&"number"==typeof e.length)return r&&(e=r),n=0,{s:t=function(){},n:function(){return n>=e.length?{done:!0}:{done:!1,value:e[n++]}},e:function(e){throw e},f:t};throw new TypeError("Invalid attempt to iterate non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.")}var a,o=!0,i=!1;return{s:function(){r=r.call(e)},n:function(){var e=r.next();return o=e.done,e},e:function(e){i=!0,a=e},f:function(){try{o||null==r.return||r.return()}finally{if(i)throw a}}}}function _unsupportedIterableToArray(e,t){if(e){if("string"==typeof e)return _arrayLikeToArray(e,t);var n=Object.prototype.toString.call(e).slice(8,-1);return"Map"===(n="Object"===n&&e.constructor?e.constructor.name:n)||"Set"===n?Array.from(e):"Arguments"===n||/^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)?_arrayLikeToArray(e,t):void 0}}function _arrayLikeToArray(e,t){(null==t||t>e.length)&&(t=e.length);for(var n=0,r=new Array(t);n<t;n++)r[n]=e[n];return r}function _classCallCheck(e,t){if(!(e instanceof t))throw new TypeError("Cannot call a class as a function")}function _defineProperties(e,t){for(var n=0;n<t.length;n++){var r=t[n];r.enumerable=r.enumerable||!1,r.configurable=!0,"value"in r&&(r.writable=!0),Object.defineProperty(e,r.key,r)}}function _createClass(e,t,n){return t&&_defineProperties(e.prototype,t),n&&_defineProperties(e,n),Object.defineProperty(e,"prototype",{writable:!1}),e}var Game=function(){var c,l={gameState:null,prev:null,gameId:"02af7930-6faf-45a8-bf90-961cc5e40beb",playerId:"",mvcURL:""};function n(){return(1===l.gameState.aanDeBeurt?l.gameState.speler1Token:l.gameState.speler2Token)===l.playerId}function r(){var i=null===l.gameState;Game.Model.getGameState(l.gameId).then(function(e){if(Game.Model.getAfgelopen(l.gameId).then(function(e){e&&(f(),clearInterval(c))}).catch(function(e){return console.error(e)}),l.gameState=e,u(),i)Game.Reversi.renderBoard(l.gameState.bord);else{for(var t=!1,n=0;n<8;n++)for(var r=0;r<8;r++){var a=l.prev.bord[r][n],o=l.gameState.bord[r][n];a!==o&&(t=!0,Game.Reversi.showFiche(n,r,o))}t&&(s(),Game.Stats.addData(l.gameState))}l.prev=l.gameState}).catch(function(e){return console.error(e)})}var u=function(){var e=document.getElementById("beurt"),t=n();t?($(e).removeClass("beurt--jouw"),$(e).addClass("beurt--mijn")):($(e).removeClass("beurt--mijn"),$(e).addClass("beurt--jouw")),e.innerText=t?"Je bent aan de beurt":"De ander is aan de beurt"},f=function(){var e=l.gameState.bord.map(function(e){return e.filter(function(e){return 1===e}).length}).reduce(function(e,t){return e+t}),t=l.gameState.bord.map(function(e){return e.filter(function(e){return 2===e}).length}).reduce(function(e,t){return e+t}),n=l.gameState.speler1Token===l.playerId,r=document.getElementById("beurt"),r=(r.innerHTML=t<e?n?"🔥🎊🔥🙌🎈🍻 Gewonnen! 🔥🎊🔥🙌🎈🍻":"Je hebt verloren 😢😔":e<t?n?"Je hebt verloren 😢😔":"🔥🎊🔥🙌🎈🍻 Gewonnen! 🔥🎊🔥🙌🎈🍻":"Gelijk spel",document.getElementById("leaveButton"));r.innerText="Spel Verlaten ✅🏁",r.removeAttribute("onclick"),r.onclick=function(e){$.get("".concat(l.mvcURL,"/Spel/Finish/").concat(l.gameId)).then(function(e){return window.location="/"})}},s=function(){var e=l.gameState.bord.map(function(e){return e.filter(function(e){return 1===e}).length}).reduce(function(e,t){return e+t}),t=l.gameState.bord.map(function(e){return e.filter(function(e){return 2===e}).length}).reduce(function(e,t){return e+t}),n=document.getElementById("score");"2 - 2"!=="".concat(e," - ").concat(t)&&(n.innerText="".concat(e," - ").concat(t))};return{init:function(e,t,n){l.gameId=e,l.playerId=t,l.mvcURL=n,Game.Data.init("".concat(l.mvcURL),"production"),Game.Model.init(),Game.Template.init(),Game.API.init(),Game.Stats.init(),Game.Reversi.init(),document.getElementById("leaveButton").onclick=function(e){1==confirm("Weet je zeker dat je het spel wilt verlaten?")&&$.get("".concat(l.mvcURL,"/Spel/Verlaten/").concat(l.gameId,"?spelerToken=").concat(l.playerId)).then(function(e){return window.location="/"})},r(),c=setInterval(r,2e3)},placeFiche:function(e,t){if(document.getElementById("beurt").innerText="Laden...",n())return Game.Data.put("/api/spel/".concat(l.gameId,"/zet?token=").concat(l.playerId,"&kolom=").concat(e,"&rij=").concat(t)),""}}}(),FeedbackWidget=function(){function t(e){_classCallCheck(this,t),this._elementId=e}return _createClass(t,[{key:"elementId",get:function(){return this._elementId}},{key:"show",value:function(e,t){var n=$("#"+this._elementId),r=$("#alert-message");$(n).fadeIn("slow"),r.text(e),r.addClass("yeet"),"danger"===t?($(n).addClass("feedback-widget--state-danger"),$(n).removeClass("feedback-widget--state-succes")):"success"===t&&($(n).addClass("feedback-widget--state-succes"),$(n).removeClass("feedback-widget--state-danger")),this.log({message:e,type:t})}},{key:"hide",value:function(){var e=document.getElementById(this._elementId);$(e).fadeOut("slow")}},{key:"log",value:function(e){var t=JSON.parse(localStorage.getItem("feedback_widget"));null==t?localStorage.setItem("feedback_widget",JSON.stringify([e])):(t.push(e),10<t.length&&t.shift(),this.removelog(),localStorage.setItem("feedback_widget",JSON.stringify(t)))}},{key:"removelog",value:function(){localStorage.removeItem("feedback_widget")}},{key:"history",value:function(){var e=(null!=(e=JSON.parse(localStorage.getItem("feedback_widget")))?e:[]).map(function(e){return"".concat(e.type," - ").concat(e.message)}).join("\n");console.log(e)}}]),t}();Game.API={init:function(){console.log("from game.api.init!")}},Game.Data=function(){function t(t){var n=r.mock.find(function(e){return e.url===t}).data;return new Promise(function(e,t){e(n)})}var n={environment:"development"},r={url:"",apiKey:"<plaats hier je apikey>",mock:[{url:"/api/spel/beurt",data:0}]};return{init:function(e,t){if(r.url=e,"production"!==t&&"development"!==t)throw new Error("environment must be production or development");n.environment=t,console.log(n.environment+" environment"),console.log("from game.data.init!")},get:function(e){return"development"===n.environment?t(e):"production"===n.environment?$.get(r.url+e).then(function(e){return e}).catch(function(e){console.log(e.message)}):void 0},put:function(e){return"development"===n.environment?t(e):new Promise(function(t){$.ajax({url:r.url+e,type:"PUT",success:function(e){t(e)}})})}}}(),Game.Model={init:function(){console.log("from game.model.init!")},getGameState:function(e){return new Promise(function(u,f){console.log("getting game for speltoken: ".concat(e)),Game.Data.get("/Spel/GetSpel/".concat(null!=e?e:"")).then(function(e){for(var t,n=e.aanDeBeurt,r=(console.log(e),e),a=r.bord,o=[],i=0;i<8;i++)o.push([0,0,0,0,0,0,0,0]);for(t in a){var c=a[t],l=t.split(",");o[l[0]][l[1]]=c}r.bord=o,0===n||1===n||2===n?u(e):(console.error("Game.Model.getGameState: Error fetching gamestate"),f("Invalid gamestate or failed request."))}).catch(function(e){return f(e)})})},getAfgelopen:function(e){return new Promise(function(t,n){Game.Data.get("/Spel/Afgelopen/".concat(null!=e?e:"")).then(function(e){t(e)}).catch(function(e){return n(e)})})}},Game.Reversi=function(){var r="board.body";return{init:function(){console.log("from game.reversi.init!")},renderBoard:function(e){document.querySelector(".board").innerHTML=Game.Template.parseTemplate(r,{board:e});for(var t=function(r){for(var e=0;e<8;e++)!function(t){var n=document.querySelector(".board__row__element--y-".concat(r,".board__row__element--x-").concat(t));n.addEventListener("click",function(e){0<n.children.length||Game.placeFiche(t,r)})}(e)},n=0;n<8;n++)t(n)},showFiche:function(e,t,n){t=document.querySelector(".board__row__element--y-".concat(t,".board__row__element--x-").concat(e)),(e=document.createElement("div")).classList.add("fiche"),e.classList.add("fiche--".concat(n)),t.innerHTML="",t.append(e)}}}(),Game.Stats=function(){var n,r=[],a=[];function o(){var e,t=$("#stats");null!=(e=n)&&e.destroy(),n=new Chart(t,{type:"line",data:{labels:r.map(function(e,t){return t+1}),datasets:[{label:"Speler 1",data:r,fill:!1,borderColor:"rgb(179, 91, 91)",tension:.1},{label:"Speler 2",data:a,fill:!1,borderColor:"rgb(91, 179, 94)",tension:.1}]},options:{scales:{y:{beginAtZero:!0}}}})}return{init:function(){console.log("from game.stats.init!"),o()},addData:function(e){var t=e.bord.map(function(e){return e.filter(function(e){return 1===e}).length}).reduce(function(e,t){return e+t}),e=e.bord.map(function(e){return e.filter(function(e){return 2===e}).length}).reduce(function(e,t){return e+t});r.push(t),a.push(e),o()}}}(),Game.Template=function(){function n(e){var t,n=spa_templates.templates,r=_createForOfIteratorHelper(e.split("."));try{for(r.s();!(t=r.n()).done;)n=n[t.value]}catch(e){r.e(e)}finally{r.f()}return n}return{init:function(){Handlebars.registerHelper("ifeq",function(e,t,n){return e===t?n.fn(this):n.inverse(this)}),Handlebars.registerHelper("ifnoteq",function(e,t,n){return e!==t?n.fn(this):n.inverse(this)})},getTemplate:n,parseTemplate:function(e,t){return n(e)(t)}}}();