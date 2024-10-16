// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","./assert","./character","./messages"],function(n,m,e,k){let t=function(){function p(a,b){this.source=a;this.errorHandler=b;this.isModule=this.trackComment=!1;this.length=a.length;this.index=0;this.lineNumber=0<a.length?1:0;this.lineStart=0;this.curlyStack=[]}var g=p.prototype;g.saveState=function(){return{index:this.index,lineNumber:this.lineNumber,lineStart:this.lineStart,curlyStack:this.curlyStack.slice()}};g.restoreState=function(a){this.index=a.index;this.lineNumber=a.lineNumber;
this.lineStart=a.lineStart;this.curlyStack=a.curlyStack};g.eof=function(){return this.index>=this.length};g.throwUnexpectedToken=function(a=k.Messages.UnexpectedTokenIllegal){return this.errorHandler.throwError(this.index,this.lineNumber,this.index-this.lineStart+1,a)};g.tolerateUnexpectedToken=function(a=k.Messages.UnexpectedTokenIllegal){this.errorHandler.tolerateError(this.index,this.lineNumber,this.index-this.lineStart+1,a)};g.skipSingleLineComment=function(a){let b=[],c=0,d=null;this.trackComment&&
(b=[],c=this.index-a,d={start:{line:this.lineNumber,column:this.index-this.lineStart-a},end:{line:0,column:0}});for(;!this.eof();){const f=this.source.charCodeAt(this.index);++this.index;if(e.Character.isLineTerminator(f))return d&&(d.end={line:this.lineNumber,column:this.index-this.lineStart-1},b.push({multiLine:!1,slice:[c+a,this.index-1],range:[c,this.index-1],loc:d})),13===f&&10===this.source.charCodeAt(this.index)&&++this.index,++this.lineNumber,this.lineStart=this.index,b}d&&(d.end={line:this.lineNumber,
column:this.index-this.lineStart},b.push({multiLine:!1,slice:[c+a,this.index],range:[c,this.index],loc:d}));return b};g.skipMultiLineComment=function(){const a=[];let b=0,c=null;this.trackComment&&(b=this.index-2,c={start:{line:this.lineNumber,column:this.index-this.lineStart-2},end:{line:0,column:0}});for(;!this.eof();){const d=this.source.charCodeAt(this.index);if(e.Character.isLineTerminator(d))13===d&&10===this.source.charCodeAt(this.index+1)&&++this.index,++this.lineNumber,++this.index,this.lineStart=
this.index;else{if(42===d&&47===this.source.charCodeAt(this.index+1))return this.index+=2,c&&(c.end={line:this.lineNumber,column:this.index-this.lineStart},a.push({multiLine:!0,slice:[b+2,this.index-2],range:[b,this.index],loc:c})),a;++this.index}}c&&(c.end={line:this.lineNumber,column:this.index-this.lineStart},a.push({multiLine:!0,slice:[b+2,this.index],range:[b,this.index],loc:c}));this.tolerateUnexpectedToken();return a};g.scanComments=function(){let a=null;this.trackComment&&(a=[]);for(var b=
0===this.index;!this.eof();){var c=this.source.charCodeAt(this.index);if(e.Character.isWhiteSpace(c))++this.index;else if(e.Character.isLineTerminator(c))++this.index,13===c&&10===this.source.charCodeAt(this.index)&&++this.index,++this.lineNumber,this.lineStart=this.index,b=!0;else if(47===c)if(c=this.source.charCodeAt(this.index+1),47===c)this.index+=2,b=this.skipSingleLineComment(2),a&&(a=a.concat(b)),b=!0;else if(42===c)this.index+=2,c=this.skipMultiLineComment(),a&&(a=a.concat(c));else break;
else if(b&&45===c)if(45===this.source.charCodeAt(this.index+1)&&62===this.source.charCodeAt(this.index+2))this.index+=3,c=this.skipSingleLineComment(3),a&&(a=a.concat(c));else break;else if(60!==c||this.isModule)break;else if("!--"===this.source.slice(this.index+1,this.index+4))this.index+=4,c=this.skipSingleLineComment(4),a&&(a=a.concat(c));else break}return a};g.isKeyword=function(a){a=a.toLowerCase();switch(a.length){case 2:return"if"===a||"in"===a;case 3:return"var"===a||"for"===a;case 4:return"else"===
a;case 5:return"break"===a;case 6:return"return"===a;case 8:return"function"===a||"continue"===a;default:return!1}};g.codePointAt=function(a){let b=this.source.charCodeAt(a);55296<=b&&56319>=b&&(a=this.source.charCodeAt(a+1),56320<=a&&57343>=a&&(b=1024*(b-55296)+a-56320+65536));return b};g.scanHexEscape=function(a){a="u"===a?4:2;let b=0;for(let c=0;c<a;++c)if(!this.eof()&&e.Character.isHexDigit(this.source.charCodeAt(this.index)))b=16*b+"0123456789abcdef".indexOf(this.source[this.index++].toLowerCase());
else return null;return String.fromCharCode(b)};g.scanUnicodeCodePointEscape=function(){let a=this.source[this.index],b=0;for("}"===a&&this.throwUnexpectedToken();!this.eof();){a=this.source[this.index++];if(!e.Character.isHexDigit(a.charCodeAt(0)))break;b=16*b+"0123456789abcdef".indexOf(a.toLowerCase())}(1114111<b||"}"!==a)&&this.throwUnexpectedToken();return e.Character.fromCodePoint(b)};g.getIdentifier=function(){const a=this.index++;for(;!this.eof();){const b=this.source.charCodeAt(this.index);
if(92===b||55296<=b&&57343>b)return this.index=a,this.getComplexIdentifier();if(e.Character.isIdentifierPart(b))++this.index;else break}return this.source.slice(a,this.index)};g.getComplexIdentifier=function(){let a=this.codePointAt(this.index),b=e.Character.fromCodePoint(a);this.index+=b.length;let c;92===a&&(117!==this.source.charCodeAt(this.index)&&this.throwUnexpectedToken(),++this.index,"{"===this.source[this.index]?(++this.index,c=this.scanUnicodeCodePointEscape()):(c=this.scanHexEscape("u"),
null!==c&&"\\"!==c&&e.Character.isIdentifierStart(c.charCodeAt(0))||this.throwUnexpectedToken()),b=c);for(;!this.eof();){a=this.codePointAt(this.index);if(!e.Character.isIdentifierPart(a))break;c=e.Character.fromCodePoint(a);b+=c;this.index+=c.length;92===a&&(b=b.substr(0,b.length-1),117!==this.source.charCodeAt(this.index)&&this.throwUnexpectedToken(),++this.index,"{"===this.source[this.index]?(++this.index,c=this.scanUnicodeCodePointEscape()):(c=this.scanHexEscape("u"),null!==c&&"\\"!==c&&e.Character.isIdentifierPart(c.charCodeAt(0))||
this.throwUnexpectedToken()),b+=c)}return b};g.octalToDecimal=function(a){let b="0"!==a,c="01234567".indexOf(a);!this.eof()&&e.Character.isOctalDigit(this.source.charCodeAt(this.index))&&(b=!0,c=8*c+"01234567".indexOf(this.source[this.index++]),0<="0123".indexOf(a)&&!this.eof()&&e.Character.isOctalDigit(this.source.charCodeAt(this.index))&&(c=8*c+"01234567".indexOf(this.source[this.index++])));return{code:c,octal:b}};g.scanIdentifier=function(){let a;const b=this.index,c=92===this.source.charCodeAt(b)?
this.getComplexIdentifier():this.getIdentifier();a=1===c.length?3:this.isKeyword(c)?4:"null"===c.toLowerCase()?5:"true"===c.toLowerCase()||"false"===c.toLowerCase()?1:3;if(3!==a&&b+c.length!==this.index){const d=this.index;this.index=b;this.tolerateUnexpectedToken(k.Messages.InvalidEscapedReservedWord);this.index=d}return{type:a,value:c,lineNumber:this.lineNumber,lineStart:this.lineStart,start:b,end:this.index}};g.scanPunctuator=function(){const a=this.index;let b=this.source[this.index];switch(b){case "(":case "{":"{"===
b&&this.curlyStack.push("{");++this.index;break;case ".":++this.index;break;case "}":++this.index;this.curlyStack.pop();break;case ")":case ";":case ",":case "[":case "]":case ":":case "?":case "~":++this.index;break;default:b=this.source.substr(this.index,4),"\x3e\x3e\x3e\x3d"===b?this.index+=4:(b=b.substr(0,3),"\x3d\x3d\x3d"===b||"!\x3d\x3d"===b||"\x3e\x3e\x3e"===b||"\x3c\x3c\x3d"===b||"\x3e\x3e\x3d"===b||"**\x3d"===b?this.index+=3:(b=b.substr(0,2),"\x26\x26"===b||"||"===b||"\x3d\x3d"===b||"!\x3d"===
b||"+\x3d"===b||"-\x3d"===b||"*\x3d"===b||"/\x3d"===b||"++"===b||"--"===b||"\x3c\x3c"===b||"\x3e\x3e"===b||"\x26\x3d"===b||"|\x3d"===b||"^\x3d"===b||"%\x3d"===b||"\x3c\x3d"===b||"\x3e\x3d"===b||"\x3d\x3e"===b||"**"===b?this.index+=2:(b=this.source[this.index],0<="\x3c\x3e\x3d!+-*%\x26|^/".indexOf(b)&&++this.index)))}this.index===a&&this.throwUnexpectedToken();return{type:7,value:b,lineNumber:this.lineNumber,lineStart:this.lineStart,start:a,end:this.index}};g.scanHexLiteral=function(a){let b="";for(;!this.eof()&&
e.Character.isHexDigit(this.source.charCodeAt(this.index));)b+=this.source[this.index++];0===b.length&&this.throwUnexpectedToken();e.Character.isIdentifierStart(this.source.charCodeAt(this.index))&&this.throwUnexpectedToken();return{type:6,value:parseInt("0x"+b,16),lineNumber:this.lineNumber,lineStart:this.lineStart,start:a,end:this.index}};g.scanBinaryLiteral=function(a){let b="";for(;!this.eof();){var c=this.source[this.index];if("0"!==c&&"1"!==c)break;b+=this.source[this.index++]}0===b.length&&
this.throwUnexpectedToken();this.eof()||(c=this.source.charCodeAt(this.index),(e.Character.isIdentifierStart(c)||e.Character.isDecimalDigit(c))&&this.throwUnexpectedToken());return{type:6,value:parseInt(b,2),lineNumber:this.lineNumber,lineStart:this.lineStart,start:a,end:this.index}};g.scanOctalLiteral=function(a,b){let c="",d=!1;e.Character.isOctalDigit(a.charCodeAt(0))?(d=!0,c="0"+this.source[this.index++]):++this.index;for(;!this.eof()&&e.Character.isOctalDigit(this.source.charCodeAt(this.index));)c+=
this.source[this.index++];d||0!==c.length||this.throwUnexpectedToken();(e.Character.isIdentifierStart(this.source.charCodeAt(this.index))||e.Character.isDecimalDigit(this.source.charCodeAt(this.index)))&&this.throwUnexpectedToken();return{type:6,value:parseInt(c,8),octal:d,lineNumber:this.lineNumber,lineStart:this.lineStart,start:b,end:this.index}};g.scanNumericLiteral=function(){const a=this.index;let b=this.source[a];m.assert(e.Character.isDecimalDigit(b.charCodeAt(0))||"."===b,"Numeric literal must start with a decimal digit or a decimal point");
let c="";if("."!==b){c=this.source[this.index++];b=this.source[this.index];if("0"===c){if("x"===b||"X"===b)return++this.index,this.scanHexLiteral(a);if("b"===b||"B"===b)return++this.index,this.scanBinaryLiteral(a);if("o"===b||"O"===b)return this.scanOctalLiteral(b,a)}for(;e.Character.isDecimalDigit(this.source.charCodeAt(this.index));)c+=this.source[this.index++];b=this.source[this.index]}if("."===b){for(c+=this.source[this.index++];e.Character.isDecimalDigit(this.source.charCodeAt(this.index));)c+=
this.source[this.index++];b=this.source[this.index]}if("e"===b||"E"===b){c+=this.source[this.index++];b=this.source[this.index];if("+"===b||"-"===b)c+=this.source[this.index++];if(e.Character.isDecimalDigit(this.source.charCodeAt(this.index)))for(;e.Character.isDecimalDigit(this.source.charCodeAt(this.index));)c+=this.source[this.index++];else this.throwUnexpectedToken()}e.Character.isIdentifierStart(this.source.charCodeAt(this.index))&&this.throwUnexpectedToken();return{type:6,value:parseFloat(c),
lineNumber:this.lineNumber,lineStart:this.lineStart,start:a,end:this.index}};g.scanStringLiteral=function(){const a=this.index;let b=this.source[a];m.assert("'"===b||'"'===b,"String literal must starts with a quote");++this.index;let c=!1,d="";for(;!this.eof();){var f=this.source[this.index++];if(f===b){b="";break}if("\\"===f)if((f=this.source[this.index++])&&e.Character.isLineTerminator(f.charCodeAt(0)))++this.lineNumber,"\r"===f&&"\n"===this.source[this.index]&&++this.index,this.lineStart=this.index;
else switch(f){case "u":"{"===this.source[this.index]?(++this.index,d+=this.scanUnicodeCodePointEscape()):(f=this.scanHexEscape(f),null===f&&this.throwUnexpectedToken(),d+=f);break;case "x":f=this.scanHexEscape(f);null===f&&this.throwUnexpectedToken(k.Messages.InvalidHexEscapeSequence);d+=f;break;case "n":d+="\n";break;case "r":d+="\r";break;case "t":d+="\t";break;case "b":d+="\b";break;case "f":d+="\f";break;case "v":d+="\v";break;case "8":case "9":d+=f;this.tolerateUnexpectedToken();break;default:f&&
e.Character.isOctalDigit(f.charCodeAt(0))?(f=this.octalToDecimal(f),c=f.octal||c,d+=String.fromCharCode(f.code)):d+=f}else if(e.Character.isLineTerminator(f.charCodeAt(0)))break;else d+=f}""!==b&&(this.index=a,this.throwUnexpectedToken());return{type:8,value:d,octal:c,lineNumber:this.lineNumber,lineStart:this.lineStart,start:a,end:this.index}};g.scanTemplate=function(){let a="",b=!1;const c=this.index,d="`"===this.source[c];let f=!1,l=2;for(++this.index;!this.eof();){var h=this.source[this.index++];
if("`"===h){l=1;b=f=!0;break}if("$"===h){if("{"===this.source[this.index]){this.curlyStack.push("${");++this.index;b=!0;break}a+=h}else if("\\"===h)if(h=this.source[this.index++],e.Character.isLineTerminator(h.charCodeAt(0)))++this.lineNumber,"\r"===h&&"\n"===this.source[this.index]&&++this.index,this.lineStart=this.index;else switch(h){case "n":a+="\n";break;case "r":a+="\r";break;case "t":a+="\t";break;case "u":if("{"===this.source[this.index])++this.index,a+=this.scanUnicodeCodePointEscape();else{const r=
this.index,q=this.scanHexEscape(h);null!==q?a+=q:(this.index=r,a+=h)}break;case "x":h=this.scanHexEscape(h);null===h&&this.throwUnexpectedToken(k.Messages.InvalidHexEscapeSequence);a+=h;break;case "b":a+="\b";break;case "f":a+="\f";break;case "v":a+="\v";break;default:"0"===h?(e.Character.isDecimalDigit(this.source.charCodeAt(this.index))&&this.throwUnexpectedToken(k.Messages.TemplateOctalLiteral),a+="\x00"):e.Character.isOctalDigit(h.charCodeAt(0))?this.throwUnexpectedToken(k.Messages.TemplateOctalLiteral):
a+=h}else e.Character.isLineTerminator(h.charCodeAt(0))?(++this.lineNumber,"\r"===h&&"\n"===this.source[this.index]&&++this.index,this.lineStart=this.index,a+="\n"):a+=h}b||this.throwUnexpectedToken();d||this.curlyStack.pop();return{type:10,value:this.source.slice(c+1,this.index-l),cooked:a,head:d,tail:f,lineNumber:this.lineNumber,lineStart:this.lineStart,start:c,end:this.index}};g.testRegExp=function(a,b){let c=a;0<=b.indexOf("u")&&(c=c.replace(/\\u\{([0-9a-fA-F]+)\}|\\u([a-fA-F0-9]{4})/g,(d,f,l)=>
{d=parseInt(f||l,16);1114111<d&&this.throwUnexpectedToken(k.Messages.InvalidRegExp);return 65535>=d?String.fromCharCode(d):"\uffff"}).replace(/[\uD800-\uDBFF][\uDC00-\uDFFF]/g,"\uffff"));try{RegExp(c)}catch(d){this.throwUnexpectedToken(k.Messages.InvalidRegExp)}try{return new RegExp(a,b)}catch(d){return null}};g.scanRegExpBody=function(){let a=this.source[this.index];m.assert("/"===a,"Regular expression literal must start with a slash");let b=this.source[this.index++],c=!1,d=!1;for(;!this.eof();)if(a=
this.source[this.index++],b+=a,"\\"===a)a=this.source[this.index++],e.Character.isLineTerminator(a.charCodeAt(0))&&this.throwUnexpectedToken(k.Messages.UnterminatedRegExp),b+=a;else if(e.Character.isLineTerminator(a.charCodeAt(0)))this.throwUnexpectedToken(k.Messages.UnterminatedRegExp);else if(c)"]"===a&&(c=!1);else{if("/"===a){d=!0;break}"["===a&&(c=!0)}d||this.throwUnexpectedToken(k.Messages.UnterminatedRegExp);return b.substr(1,b.length-2)};g.scanRegExpFlags=function(){let a="";for(;!this.eof();){var b=
this.source[this.index];if(!e.Character.isIdentifierPart(b.charCodeAt(0)))break;++this.index;if("\\"!==b||this.eof())a+=b;else{b=this.source[this.index];if("u"===b){++this.index;b=this.index;const c=this.scanHexEscape("u");if(null!==c)for(a+=c;b<this.index;++b);else this.index=b,a+="u"}this.tolerateUnexpectedToken()}}return a};g.scanRegExp=function(){const a=this.index,b=this.scanRegExpBody(),c=this.scanRegExpFlags(),d=this.testRegExp(b,c);return{type:9,value:"",pattern:b,flags:c,regex:d,lineNumber:this.lineNumber,
lineStart:this.lineStart,start:a,end:this.index}};g.lex=function(){if(this.eof())return{type:2,value:"",lineNumber:this.lineNumber,lineStart:this.lineStart,start:this.index,end:this.index};const a=this.source.charCodeAt(this.index);return e.Character.isIdentifierStart(a)?this.scanIdentifier():40===a||41===a||59===a?this.scanPunctuator():39===a||34===a?this.scanStringLiteral():46===a?e.Character.isDecimalDigit(this.source.charCodeAt(this.index+1))?this.scanNumericLiteral():this.scanPunctuator():e.Character.isDecimalDigit(a)?
this.scanNumericLiteral():96===a||125===a&&"${"===this.curlyStack[this.curlyStack.length-1]?this.scanTemplate():55296<=a&&57343>a&&e.Character.isIdentifierStart(this.codePointAt(this.index))?this.scanIdentifier():this.scanPunctuator()};return p}();n.Scanner=t;Object.defineProperty(n,"__esModule",{value:!0})});