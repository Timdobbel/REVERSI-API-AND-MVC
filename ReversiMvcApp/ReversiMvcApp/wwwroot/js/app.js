"use strict";function _createForOfIteratorHelper(e,t){var n,r="undefined"!=typeof Symbol&&e[Symbol.iterator]||e["@@iterator"];if(!r){if(Array.isArray(e)||(r=_unsupportedIterableToArray(e))||t&&e&&"number"==typeof e.length)return r&&(e=r),n=0,{s:t=function(){},n:function(){return n>=e.length?{done:!0}:{done:!1,value:e[n++]}},e:function(e){throw e},f:t};throw new TypeError("Invalid attempt to iterate non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.")}var a,o=!0,i=!1;return{s:function(){r=r.call(e)},n:function(){var e=r.next();return o=e.done,e},e:function(e){i=!0,a=e},f:function(){try{o||null==r.return||r.return()}finally{if(i)throw a}}}}function _unsupportedIterableToArray(e,t){if(e){if("string"==typeof e)return _arrayLikeToArray(e,t);var n=Object.prototype.toString.call(e).slice(8,-1);return"Map"===(n="Object"===n&&e.constructor?e.constructor.name:n)||"Set"===n?Array.from(e):"Arguments"===n||/^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)?_arrayLikeToArray(e,t):void 0}}function _arrayLikeToArray(e,t){(null==t||t>e.length)&&(t=e.length);for(var n=0,r=new Array(t);n<t;n++)r[n]=e[n];return r}function _classCallCheck(e,t){if(!(e instanceof t))throw new TypeError("Cannot call a class as a function")}function _defineProperties(e,t){for(var n=0;n<t.length;n++){var r=t[n];r.enumerable=r.enumerable||!1,r.configurable=!0,"value"in r&&(r.writable=!0),Object.defineProperty(e,r.key,r)}}function _createClass(e,t,n){return t&&_defineProperties(e.prototype,t),n&&_defineProperties(e,n),Object.defineProperty(e,"prototype",{writable:!1}),e}function _typeof(e){return(_typeof="function"==typeof Symbol&&"symbol"==typeof Symbol.iterator?function(e){return typeof e}:function(e){return e&&"function"==typeof Symbol&&e.constructor===Symbol&&e!==Symbol.prototype?"symbol":typeof e})(e)}var Game=function(){var c,l={gameState:null,prev:null,gameId:"02af7930-6faf-45a8-bf90-961cc5e40beb",playerId:"",mvcURL:""};function n(){return(1===l.gameState.aanDeBeurt?l.gameState.speler1Token:l.gameState.speler2Token)===l.playerId}function r(e){document.getElementById("beurt").innerText=e}function a(){var i=null===l.gameState;Game.Model.getGameState(l.gameId).then(function(e){if(null===e)return d(),void clearInterval(c);if(Game.Model.getAfgelopen(l.gameId).then(function(e){e&&(s(),clearInterval(c))}).catch(function(e){return console.error(e)}),l.gameState=e,u(),i)Game.Reversi.renderBoard(l.gameState.bord);else{for(var t=!1,n=0;n<8;n++)for(var r=0;r<8;r++){var a=l.prev.bord[r][n],o=l.gameState.bord[r][n];a!==o&&(t=!0,Game.Reversi.showFiche(n,r,o))}t&&(f(),Game.Stats.addData(l.gameState))}l.prev=l.gameState}).catch(function(e){return console.error(e)})}var u=function(){var e=document.getElementById("beurt"),t=n();null==l.gameState.speler2Token?r("Wachtend op een tegenstander"):(t?($(e).removeClass("beurt--jouw"),$(e).addClass("beurt--mijn")):($(e).removeClass("beurt--mijn"),$(e).addClass("beurt--jouw")),r(t?"Je bent aan de beurt":"De ander is aan de beurt"))},s=function(){var e=l.gameState.bord.map(function(e){return e.filter(function(e){return 1===e}).length}).reduce(function(e,t){return e+t}),t=l.gameState.bord.map(function(e){return e.filter(function(e){return 2===e}).length}).reduce(function(e,t){return e+t}),n=l.gameState.speler1Token===l.playerId,r=document.getElementById("beurt"),e=(t<e?n?(Game.ConfettiGenerator().render(),r.innerHTML="🔥🎊🔥🙌🎈🍻 Gewonnen! 🔥🎊🔥🙌🎈🍻",$(r).removeClass("beurt--jouw"),$(r).addClass("beurt--mijn")):(r.innerHTML="Je hebt verloren 😢😔",$(r).removeClass("beurt--mijn"),$(r).addClass("beurt--jouw ")):e<t?n?(r.innerHTML="Je hebt verloren 😢😔",$(r).removeClass("beurt--mijn"),$(r).addClass("beurt--jouw ")):(Game.ConfettiGenerator().render(),r.innerHTML="🔥🎊🔥🙌🎈🍻 Gewonnen! 🔥🎊🔥🙌🎈🍻",$(r).removeClass("beurt--jouw"),$(r).addClass("beurt--mijn")):r.innerHTML="Gelijk spel",document.getElementById("leaveButton"));e.innerText="Spel Verlaten ✅🏁",e.removeAttribute("onclick"),e.onclick=function(e){window.location="/",$.get("".concat(l.mvcURL,"/Spel/Finish/").concat(l.gameId))}},d=function(){document.getElementById("beurt").innerHTML="Tegenstander heeft het spel verlaten 😒";var e=document.getElementById("leaveButton");e.innerText="Terug naar hoofdscherm",e.removeAttribute("onclick"),e.onclick=function(e){window.location="/"}},f=function(){var e=l.gameState.bord.map(function(e){return e.filter(function(e){return 1===e}).length}).reduce(function(e,t){return e+t}),t=l.gameState.bord.map(function(e){return e.filter(function(e){return 2===e}).length}).reduce(function(e,t){return e+t}),n=document.getElementById("score");"2 - 2"!=="".concat(e," - ").concat(t)&&(n.innerText="".concat(e," - ").concat(t))};return{init:function(e,t,n){l.gameId=e,l.playerId=t,l.mvcURL=n,Game.Data.init("".concat(l.mvcURL),"production"),Game.Model.init(),Game.Template.init(),Game.API.init(),Game.Stats.init(),Game.Reversi.init(),document.getElementById("leaveButton").onclick=function(e){1==confirm("Weet je zeker dat je het spel wilt verlaten?")&&$.get("".concat(l.mvcURL,"/Spel/Verlaten/").concat(l.gameId,"?spelerToken=").concat(l.playerId)).then(function(e){return window.location="/"})},a(),c=setInterval(a,2e3)},placeFiche:function(e,t){if(document.getElementById("beurt").innerText="Laden...",n())return Game.Data.put("/api/spel/".concat(l.gameId,"/zet?token=").concat(l.playerId,"&kolom=").concat(e,"&rij=").concat(t)),""}}}(),FeedbackWidget=(Game.ConfettiGenerator=function(e){var l={target:"confetti-holder",max:600,size:1,animate:!0,respawn:!0,props:["circle","square","triangle","line"],colors:[[165,104,246],[230,61,135],[0,199,228],[253,214,126]],clock:25,interval:null,rotate:!0,start_from_edge:!1,width:window.innerWidth,height:window.innerHeight};if(e&&(e.target&&(l.target=e.target),e.max&&(l.max=e.max),e.size&&(l.size=e.size),void 0!==e.animate&&null!==e.animate&&(l.animate=e.animate),void 0!==e.respawn&&null!==e.respawn&&(l.respawn=e.respawn),e.props&&(l.props=e.props),e.colors&&(l.colors=e.colors),e.clock&&(l.clock=e.clock),void 0!==e.start_from_edge&&null!==e.start_from_edge&&(l.start_from_edge=e.start_from_edge),e.width&&(l.width=e.width),e.height&&(l.height=e.height),void 0!==e.rotate&&null!==e.rotate&&(l.rotate=e.rotate)),"object"!=_typeof(l.target)&&"string"!=typeof l.target)throw new TypeError("The target parameter should be a node or string");if("object"==_typeof(l.target)&&(null===l.target||!l.target instanceof HTMLCanvasElement)||"string"==typeof l.target&&(null===document.getElementById(l.target)||!document.getElementById(l.target)instanceof HTMLCanvasElement))throw new ReferenceError("The target element does not exist or is not a canvas element");var t="object"==_typeof(l.target)?l.target:document.getElementById(l.target),u=t.getContext("2d"),s=[];function d(e,t){e=e||1;e=Math.random()*e;return t?Math.floor(e):e}var r=l.props.reduce(function(e,t){return e+(t.weight||1)},0);function n(){var e=l.props[function(){for(var e=Math.random()*r,t=0;t<l.props.length;++t){var n=l.props[t].weight||1;if(e<n)return t;e-=n}}()];return{prop:e.type||e,x:d(l.width),y:l.start_from_edge?0<=l.clock?-10:parseFloat(l.height)+10:d(l.height),src:e.src,radius:d(4)+1,size:e.size,rotate:l.rotate,line:Math.floor(d(65)-30),angles:[d(10,!0)+2,d(10,!0)+2,d(10,!0)+2,d(10,!0)+2],color:l.colors[d(l.colors.length,!0)],rotation:d(360,!0)*Math.PI/180,speed:d(l.clock/7)+l.clock/30}}function f(){l.animate=!1,clearInterval(l.interval),requestAnimationFrame(function(){u.clearRect(0,0,t.width,t.height);var e=t.width;t.width=1,t.width=e})}return{render:function(){t.width=l.width,t.height=l.height,s=[];for(var e=0;e<l.max;e++)s.push(n());return requestAnimationFrame(function e(){for(var t in u.clearRect(0,0,l.width,l.height),s){o=a=r=n=void 0;var n=s[t];if(n){var r=n.radius<=3?.4:.8;switch(u.fillStyle=u.strokeStyle="rgba("+n.color+", "+r+")",u.beginPath(),n.prop){case"circle":u.moveTo(n.x,n.y),u.arc(n.x,n.y,n.radius*l.size,0,2*Math.PI,!0),u.fill();break;case"triangle":u.moveTo(n.x,n.y),u.lineTo(n.x+n.angles[0]*l.size,n.y+n.angles[1]*l.size),u.lineTo(n.x+n.angles[2]*l.size,n.y+n.angles[3]*l.size),u.closePath(),u.fill();break;case"line":u.moveTo(n.x,n.y),u.lineTo(n.x+n.line*l.size,n.y+5*n.radius),u.lineWidth=2*l.size,u.stroke();break;case"square":u.save(),u.translate(n.x+15,n.y+5),u.rotate(n.rotation),u.fillRect(-15*l.size,-5*l.size,15*l.size,5*l.size),u.restore();break;case"svg":u.save();var a=new window.Image,o=(a.src=n.src,n.size||15);u.translate(n.x+o/2,n.y+o/2),n.rotate&&u.rotate(n.rotation),u.drawImage(a,-o/2*l.size,-o/2*l.size,o*l.size,o*l.size),u.restore()}}}for(var i=0;i<l.max;i++){var c=s[i];c&&(l.animate&&(c.y+=c.speed),c.rotate&&(c.rotation+=c.speed/35),(0<=c.speed&&c.y>l.height||c.speed<0&&c.y<0)&&(l.respawn?(s[i]=c,s[i].x=d(l.width,!0),s[i].y=0<=c.speed?-10:parseFloat(l.height)):s[i]=void 0))}s.every(function(e){return void 0===e})&&f(),l.animate&&requestAnimationFrame(e)})},clear:f}},function(){function t(e){_classCallCheck(this,t),this._elementId=e}return _createClass(t,[{key:"elementId",get:function(){return this._elementId}},{key:"show",value:function(e,t){var n=$("#"+this._elementId),r=$("#alert-message");$(n).fadeIn("slow"),r.text(e),r.addClass("yeet"),"danger"===t?($(n).addClass("feedback-widget--state-danger"),$(n).removeClass("feedback-widget--state-succes")):"success"===t&&($(n).addClass("feedback-widget--state-succes"),$(n).removeClass("feedback-widget--state-danger")),this.log({message:e,type:t})}},{key:"hide",value:function(){var e=document.getElementById(this._elementId);$(e).fadeOut("slow")}},{key:"log",value:function(e){var t=JSON.parse(localStorage.getItem("feedback_widget"));null==t?localStorage.setItem("feedback_widget",JSON.stringify([e])):(t.push(e),10<t.length&&t.shift(),this.removelog(),localStorage.setItem("feedback_widget",JSON.stringify(t)))}},{key:"removelog",value:function(){localStorage.removeItem("feedback_widget")}},{key:"history",value:function(){var e=(null!=(e=JSON.parse(localStorage.getItem("feedback_widget")))?e:[]).map(function(e){return"".concat(e.type," - ").concat(e.message)}).join("\n");console.log(e)}}]),t}());Game.API={init:function(){console.log("from game.api.init!")}},Game.Data=function(){function t(t){var n=r.mock.find(function(e){return e.url===t}).data;return new Promise(function(e,t){e(n)})}var n={environment:"development"},r={url:"",apiKey:"<plaats hier je apikey>",mock:[{url:"/api/spel/beurt",data:0}]};return{init:function(e,t){if(r.url=e,"production"!==t&&"development"!==t)throw new Error("environment must be production or development");n.environment=t,console.log(n.environment+" environment"),console.log("from game.data.init!")},get:function(e){return"development"===n.environment?t(e):"production"===n.environment?new Promise(function(t,n){$.ajax({url:r.url+e,type:"GET",success:function(e){t(e)},error:function(e){n(e)}})}):void 0},put:function(e){return"development"===n.environment?t(e):new Promise(function(t){$.ajax({url:r.url+e,type:"PUT",success:function(e){t(e)}})})}}}(),Game.Model={init:function(){console.log("from game.model.init!")},getGameState:function(e){return new Promise(function(l,u){Game.Data.get("/Spel/GetSpel/".concat(null!=e?e:"")).then(function(e){if(null===e&&l(e),0===e.aanDeBeurt||1===e.aanDeBeurt||2===e.aanDeBeurt){for(var t,n=e,r=n.bord,a=[],o=0;o<8;o++)a.push([0,0,0,0,0,0,0,0]);for(t in r){var i=r[t],c=t.split(",");a[c[0]][c[1]]=i}n.bord=a,l(e)}else console.error("Game.Model.getGameState: Error fetching gamestate"),u("Invalid gamestate or failed request.")}).catch(function(e){return u(e)})})},getAfgelopen:function(e){return new Promise(function(t,n){Game.Data.get("/Spel/Afgelopen/".concat(null!=e?e:"")).then(function(e){t(e)}).catch(function(e){return n(e)})})}},Game.Reversi=function(){var r="board.body";return{init:function(){console.log("from game.reversi.init!")},renderBoard:function(e){document.querySelector(".board").innerHTML=Game.Template.parseTemplate(r,{board:e});for(var t=function(r){for(var e=0;e<8;e++)!function(t){var n=document.querySelector(".board__row__element--y-".concat(r,".board__row__element--x-").concat(t));n.addEventListener("click",function(e){0<n.children.length||Game.placeFiche(t,r)})}(e)},n=0;n<8;n++)t(n)},showFiche:function(e,t,n){t=document.querySelector(".board__row__element--y-".concat(t,".board__row__element--x-").concat(e)),(e=document.createElement("div")).classList.add("fiche"),e.classList.add("fiche--".concat(n)),t.innerHTML="",t.append(e)}}}(),Game.Stats=function(){var n,r=[],a=[];function o(){var e,t=$("#stats");null!=(e=n)&&e.destroy(),n=new Chart(t,{type:"line",data:{labels:r.map(function(e,t){return t+1}),datasets:[{label:"Speler 1",data:r,fill:!1,borderColor:"rgb(179, 91, 91)",tension:.1},{label:"Speler 2",data:a,fill:!1,borderColor:"rgb(91, 179, 94)",tension:.1}]},options:{scales:{y:{beginAtZero:!0}}}})}return{init:function(){console.log("from game.stats.init!"),o()},addData:function(e){var t=e.bord.map(function(e){return e.filter(function(e){return 1===e}).length}).reduce(function(e,t){return e+t}),e=e.bord.map(function(e){return e.filter(function(e){return 2===e}).length}).reduce(function(e,t){return e+t});r.push(t),a.push(e),o()}}}(),Game.Template=function(){function n(e){var t,n=spa_templates.templates,r=_createForOfIteratorHelper(e.split("."));try{for(r.s();!(t=r.n()).done;)n=n[t.value]}catch(e){r.e(e)}finally{r.f()}return n}return{init:function(){Handlebars.registerHelper("ifeq",function(e,t,n){return e===t?n.fn(this):n.inverse(this)}),Handlebars.registerHelper("ifnoteq",function(e,t,n){return e!==t?n.fn(this):n.inverse(this)})},getTemplate:n,parseTemplate:function(e,t){return n(e)(t)}}}();