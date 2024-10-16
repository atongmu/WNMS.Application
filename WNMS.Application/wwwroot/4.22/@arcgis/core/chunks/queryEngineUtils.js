/*
All material copyright ESRI, All Rights Reserved, unless otherwise specified.
See https://js.arcgis.com/4.22/esri/copyright.txt for details.
*/
import{F as t,P as e,E as o}from"../widgets/Sketch/SketchViewModel.js";import{P as r}from"./PointSnappingHint.js";class n extends t{constructor(t){super({...t,constraint:new e(t.coordinateHelper,t.targetPoint)})}get hints(){return[new r(this.targetPoint)]}}function i(t,e){switch(t.type){case"edge":return new o({coordinateHelper:e,edgeStart:e.pointToVector(t.start),edgeEnd:e.pointToVector(t.end),targetPoint:e.pointToVector(t.target),objectId:t.objectId});case"vertex":return new n({coordinateHelper:e,targetPoint:e.pointToVector(t.target),objectId:t.objectId})}}export{i as c};
