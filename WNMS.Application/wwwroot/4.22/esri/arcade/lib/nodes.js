// All material copyright ESRI, All Rights Reserved, unless otherwise specified.
// See https://js.arcgis.com/4.22/esri/copyright.txt for details.
//>>built
define(["exports","../../chunks/_rollupPluginBabelHelpers","./syntax"],function(g,k,l){var h=function(){};let q=function(a){function b(e,d){var c=a.call(this)||this;c.type=e;c.value=d;return c}k._inheritsLoose(b,a);return b}(h),r=function(a){function b(e){var d=a.call(this)||this;d.elements=e;d.type=l.Syntax.ArrayExpression;return d}k._inheritsLoose(b,a);return b}(h),t=function(a){function b(e,d,c){var f=a.call(this)||this;f.operator=e;f.left=d;f.right=c;f.type=l.Syntax.AssignmentExpression;return f}
k._inheritsLoose(b,a);return b}(h);const p=["||","\x26\x26"];let u=function(a){function b(e,d,c){var f=a.call(this)||this;f.operator=e;f.left=d;f.right=c;f.type=p.includes(e)?l.Syntax.LogicalExpression:l.Syntax.BinaryExpression;return f}k._inheritsLoose(b,a);return b}(h),v=function(a){function b(e){var d=a.call(this)||this;d.body=e;d.type=l.Syntax.BlockStatement;return d}k._inheritsLoose(b,a);return b}(h),w=function(a){function b(){var e=a.apply(this,arguments)||this;e.type=l.Syntax.BreakStatement;
return e}k._inheritsLoose(b,a);return b}(h),x=function(a){function b(e,d){var c=a.call(this)||this;c.callee=e;c.args=d;c.type=l.Syntax.CallExpression;c.arguments=d;return c}k._inheritsLoose(b,a);return b}(h),y=function(a){function b(e,d){var c=a.call(this)||this;c.object=e;c.property=d;c.type=l.Syntax.MemberExpression;c.computed=!0;return c}k._inheritsLoose(b,a);return b}(h),z=function(a){function b(e,d){var c=a.call(this)||this;c.object=e;c.property=d;c.type=l.Syntax.MemberExpression;c.computed=
!1;return c}k._inheritsLoose(b,a);return b}(h),A=function(a){function b(){var e=a.apply(this,arguments)||this;e.type=l.Syntax.ContinueStatement;return e}k._inheritsLoose(b,a);return b}(h),B=function(a){function b(){var e=a.apply(this,arguments)||this;e.type=l.Syntax.EmptyStatement;return e}k._inheritsLoose(b,a);return b}(h),C=function(a){function b(e){var d=a.call(this)||this;d.expression=e;d.type=l.Syntax.ExpressionStatement;return d}k._inheritsLoose(b,a);return b}(h),D=function(a){function b(e,
d,c){var f=a.call(this)||this;f.left=e;f.right=d;f.body=c;f.type=l.Syntax.ForInStatement;f.each=!1;return f}k._inheritsLoose(b,a);return b}(h),E=function(a){function b(e,d,c,f){var m=a.call(this)||this;m.init=e;m.test=d;m.update=c;m.body=f;m.type=l.Syntax.ForStatement;return m}k._inheritsLoose(b,a);return b}(h),F=function(a){function b(e,d,c){var f=a.call(this)||this;f.id=e;f.params=d;f.body=c;f.type=l.Syntax.FunctionDeclaration;f.generator=!1;f.expression=!1;f.async=!1;return f}k._inheritsLoose(b,
a);return b}(h),G=function(a){function b(e){var d=a.call(this)||this;d.name=e;d.type=l.Syntax.Identifier;return d}k._inheritsLoose(b,a);return b}(h),H=function(a){function b(e,d,c){var f=a.call(this)||this;f.test=e;f.consequent=d;f.alternate=c;f.type=l.Syntax.IfStatement;return f}k._inheritsLoose(b,a);return b}(h),I=function(a){function b(e,d){var c=a.call(this)||this;c.value=e;c.raw=d;c.type=l.Syntax.Literal;return c}k._inheritsLoose(b,a);return b}(h),J=function(a){function b(e){var d=a.call(this)||
this;d.properties=e;d.type=l.Syntax.ObjectExpression;return d}k._inheritsLoose(b,a);return b}(h),L=function(a){function b(e,d,c,f,m,K){var n=a.call(this)||this;n.kind=e;n.key=d;n.computed=c;n.value=f;n.method=m;n.shorthand=K;n.type=l.Syntax.Property;return n}k._inheritsLoose(b,a);return b}(h),M=function(a){function b(e){var d=a.call(this)||this;d.argument=e;d.type=l.Syntax.ReturnStatement;return d}k._inheritsLoose(b,a);return b}(h),N=function(a){function b(e){var d=a.call(this)||this;d.body=e;d.type=
l.Syntax.Program;return d}k._inheritsLoose(b,a);return b}(h),O=function(a){function b(e,d){var c=a.call(this)||this;c.value=e;c.tail=d;c.type=l.Syntax.TemplateElement;return c}k._inheritsLoose(b,a);return b}(h),P=function(a){function b(e,d){var c=a.call(this)||this;c.quasis=e;c.expressions=d;c.type=l.Syntax.TemplateLiteral;return c}k._inheritsLoose(b,a);return b}(h),Q=function(a){function b(e,d){var c=a.call(this)||this;c.operator=e;c.argument=d;c.type=l.Syntax.UnaryExpression;c.prefix=!0;return c}
k._inheritsLoose(b,a);return b}(h),R=function(a){function b(e,d,c){var f=a.call(this)||this;f.operator=e;f.argument=d;f.prefix=c;f.type=l.Syntax.UpdateExpression;return f}k._inheritsLoose(b,a);return b}(h),S=function(a){function b(e,d){var c=a.call(this)||this;c.declarations=e;c.kind=d;c.type=l.Syntax.VariableDeclaration;return c}k._inheritsLoose(b,a);return b}(h);h=function(a){function b(e,d){var c=a.call(this)||this;c.id=e;c.init=d;c.type=l.Syntax.VariableDeclarator;return c}k._inheritsLoose(b,
a);return b}(h);g.ArrayExpression=r;g.AssignmentExpression=t;g.AssignmentOperators="\x3d /\x3d *\x3d %\x3d +\x3d -\x3d".split(" ");g.BinaryExpression=u;g.BinaryOperators="| \x26 \x3e\x3e \x3c\x3c \x3e\x3e \x3e\x3e\x3e ^ \x3d\x3d !\x3d \x3c \x3c\x3d \x3e \x3e\x3d + - * / %".split(" ");g.BlockStatement=v;g.BreakStatement=w;g.CallExpression=x;g.Comment=q;g.ComputedMemberExpression=y;g.ContinueStatement=A;g.EmptyStatement=B;g.ExpressionStatement=C;g.ForInStatement=D;g.ForStatement=E;g.FunctionDeclaration=
F;g.Identifier=G;g.IfStatement=H;g.Literal=I;g.LogicalOperators=p;g.ObjectExpression=J;g.Program=N;g.Property=L;g.ReturnStatement=M;g.StaticMemberExpression=z;g.TemplateElement=O;g.TemplateLiteral=P;g.UnaryExpression=Q;g.UnaryOperators=["-","+","!","~"];g.UpdateExpression=R;g.UpdateOperators=["++","--"];g.VariableDeclaration=S;g.VariableDeclarator=h;Object.defineProperty(g,"__esModule",{value:!0})});