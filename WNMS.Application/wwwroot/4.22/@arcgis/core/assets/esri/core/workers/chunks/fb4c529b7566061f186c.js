"use strict";(self.webpackChunkRemoteClient=self.webpackChunkRemoteClient||[]).push([[9047],{69047:(e,t,r)=>{r.r(t),r.d(t,{default:()=>m});var s=r(29768),a=(r(92143),r(91306),r(76506),r(34250),r(17533)),i=r(82058),o=r(62206),n=r(95533),u=r(45937),l=r(59877);r(31450),r(71552),r(88762),r(32101),r(60991),r(40642),r(50406),r(44567),r(86656),r(22723),r(2906),r(21801),r(73796),r(74673),r(21972),r(23639),r(91055),r(6906),r(97714),r(60947),r(91597),r(86787),r(35132),r(89623),r(98380),r(92896),r(94751),r(32422),r(84069),r(74569),r(22781),r(57251),r(64560),r(59465),r(23761),r(86748),r(15324),r(76996),r(14249),r(60217),r(29107),r(30574),r(2157),r(25977),r(58076),r(98242),r(7471),r(54414),r(1648),r(8925),r(33921),r(3482),r(45154),r(16769),r(55531),r(30582),r(593),r(85699),r(96055),r(47776),r(18033),r(6331),r(62048),r(4292),r(75626),r(72652),r(29641),r(30493),r(70821),r(82673),r(34229),r(37029),r(96467),r(63571),r(30776),r(48027),r(54174),r(82426),r(29794),r(63130),r(25696),r(66396),r(42775),r(95834),r(34394),r(57150),r(76726),r(20444),r(76393),r(78548),r(2497),r(49906),r(46527),r(11799),r(48649),r(98402),r(9960),r(30823),r(53326),r(92482),r(5853),r(39141),r(38742),r(48243),r(34635),r(10401),r(49900),r(22739),r(20543),r(67477),r(78533),r(74653),r(91091),r(58943),r(70737),r(8487),r(17817),r(90814),r(15459),r(61847),r(16796),r(16955),r(22401),r(77894),r(55187),r(8586),r(44509),r(69814),r(11305),r(62259),r(44790),r(5909),r(60669),r(48208),r(51589),r(87258),r(97546),r(9801),r(53523),r(42911),r(46826),r(45433),r(54732),r(37265),r(2710),r(24204),r(8021),r(69997),r(53785),r(95587);const c=(0,o.c)({accumulateAttributes:{name:"accumulateAttributeNames"},attributeParameterValues:!0,directionsTimeAttribute:{name:"directionsTimeAttributeName"},impedanceAttribute:{name:"impedanceAttributeName"},outSpatialReference:{name:"outSR",getter:e=>e.outSpatialReference.wkid},pointBarriers:{name:"barriers"},polylineBarriers:!0,polygonBarriers:!0,restrictionAttributes:{name:"restrictionAttributeNames"},stops:!0,travelMode:!0});var p=r(658);let f=class extends p.Z{constructor(e){super(e)}solve(e,t){return async function(e,t,r){const s=[],a=[],o={},p={},f=(0,l.p)(e),{path:m}=f;t.stops&&t.stops.features&&(0,u.et)(t.stops.features,a,"stops.features",o),t.pointBarriers&&t.pointBarriers.features&&(0,u.et)(t.pointBarriers.features,a,"pointBarriers.features",o),t.polylineBarriers&&t.polylineBarriers.features&&(0,u.et)(t.polylineBarriers.features,a,"polylineBarriers.features",o),t.polygonBarriers&&t.polygonBarriers.features&&(0,u.et)(t.polygonBarriers.features,a,"polygonBarriers.features",o);const y=await(0,n.aX)(a);for(const e in o){const t=o[e];s.push(e),p[e]=y.slice(t[0],t[1])}if((0,u.Wf)(p,s)){let e=null;try{e=await(0,u.bI)(m,t.apiKey,r)}catch{}e&&!e.hasZ&&(0,u.ef)(p,s)}for(const e in p)p[e].forEach(((r,s)=>{t.get(e)[s].geometry=r}));const B={...r,query:{...f.query,...c.toQueryParams(t),f:"json"}},b=m.endsWith("/solve")?m:`${m}/solve`,d=await(0,i.default)(b,B);return(0,u.mT)(d)}(this.url,e,t)}};f=(0,s._)([(0,a.j)("esri.tasks.RouteTask")],f);const m=f}}]);