!function(){var a=Handlebars.template,n=Handlebars.templates=Handlebars.templates||{};n.flatTemplateTop=a({1:function(a,n,e,r){var l,s;return'            <li data-target="#carousel-example-generic" data-slide-to="'+this.escapeExpression((s=null!=(s=n.Index||(null!=a?a.Index:a))?s:n.helperMissing,"function"==typeof s?s.call(a,{name:"Index",hash:{},data:r}):s))+'" class="'+(null!=(l=n["if"].call(a,null!=a?a.IsPrimary:a,{name:"if",hash:{},fn:this.program(2,r,0),inverse:this.noop,data:r}))?l:"")+'">\r\n                <span class="fa fa-circle-o-notch fa-1x" aria-hidden="true"></span>\r\n            </li>\r\n'},2:function(){return"active"},4:function(a,n,e,r){var l,s,t=n.helperMissing,i="function",o=this.escapeExpression;return'            <div class="item '+(null!=(l=n["if"].call(a,null!=a?a.IsPrimary:a,{name:"if",hash:{},fn:this.program(2,r,0),inverse:this.noop,data:r}))?l:"")+'">\r\n                <div style="background-image:url('+o((s=null!=(s=n.LargePath||(null!=a?a.LargePath:a))?s:t,typeof s===i?s.call(a,{name:"LargePath",hash:{},data:r}):s))+');" class="cover-photo"\r\n                     data-img-path="'+o((s=null!=(s=n.LargePath||(null!=a?a.LargePath:a))?s:t,typeof s===i?s.call(a,{name:"LargePath",hash:{},data:r}):s))+'"\r\n                     data-img-title="Уютная большая кровать"></div>\r\n                <div class="carousel-caption">\r\n                    <h2 style="color:white;"><b>Спальня</b></h2>\r\n                    <p>Уютная большая кровать</p>\r\n                </div>\r\n            </div>\r\n'},compiler:[6,">= 2.0.0-beta.1"],main:function(a,n,e,r){var l;return'    \r\n\r\n    <div id="carousel-example-generic" class="carousel slide" data-ride="carousel">\r\n        <ol class="carousel-indicators">\r\n            <li data-target="#carousel-example-generic" data-slide-to="0"\r\n                data-toggle="tooltip"\r\n                data-placement="top"\r\n                data-original-title="Карта">\r\n                <span class="fa fa-map-marker fa-2x" aria-hidden="true"></span>\r\n            </li>\r\n'+(null!=(l=n.each.call(a,null!=a?a.PhotoList:a,{name:"each",hash:{},fn:this.program(1,r,0),inverse:this.noop,data:r}))?l:"")+'        </ol>\r\n        <div class="carousel-inner" role="listbox">\r\n            <div id="mapContainer" class="item"></div>\r\n\r\n'+(null!=(l=n.each.call(a,null!=a?a.PhotoList:a,{name:"each",hash:{},fn:this.program(4,r,0),inverse:this.noop,data:r}))?l:"")+'        </div>\r\n        <a class="left carousel-control" href="#carousel-example-generic" role="button" data-slide="prev">\r\n            <span class="icon-prev" aria-hidden="true"></span>\r\n            <span class="sr-only">Previous</span>\r\n        </a>\r\n        <a class="right carousel-control" href="#carousel-example-generic" role="button" data-slide="next">\r\n            <span class="icon-next" aria-hidden="true"></span>\r\n            <span class="sr-only">Next</span>\r\n        </a>\r\n    </div>\r\n'},useData:!0})}();