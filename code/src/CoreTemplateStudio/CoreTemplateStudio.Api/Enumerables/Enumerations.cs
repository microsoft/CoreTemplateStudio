using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

namespace CoreTemplateStudio.Api.Enumerables
{
    public enum FrameworkType {
        [DisplayName("Frontend")]
        Frontend,
        [DisplayName("Backend")]
        Backend
    };

    public enum ShortProjectType
    {
        [DisplayName("Single Page Full Stack Application"), Description("A single page web application with a connected backend")]
        SPAFS,
        [DisplayName("Multi Page Full Stack Application"), Description("A multi page web application with a connected backend")]
        MPAFS,
        [DisplayName("Single Page Frontend"), Description("A single page frontend only application")]
        SPFE,
        [DisplayName("Single Page Frontend"), Description("A multi page frontend only application")]
        MPFE,
        [DisplayName("RESTful API"), Description("A REST Application programming interface backend only")]
        REST
    };

    public enum ShortFramework
    {
        [DisplayName("ReactJS"), Description("React is a JavaScript library for building user interfaces. It is maintained by Facebook and a community of individual developers")]
        RJS,
        [DisplayName("VueJS"), Description("Vue.js is an open-source JavaScript framework for building user interfaces and single-page applications.")]
        VJS,
        [DisplayName("AngularJS"), Description("AngularJS is a JavaScript - based open - source front - end web application framework mainly maintained by Google and by a community of individuals and corporations.")]
        AJS,
        [DisplayName("NodeJS"), Description("Node.js is an open-source, cross-platform JavaScript run-time environment that executes JavaScript code outside of a browser.")]
        NJS,
        [DisplayName("Django"), Description("Django is a Python-based free and open-source web framework, which follows the model-view-template architectural pattern. It is maintained by the Django Software Foundation")]
        DJG,
        [DisplayName("SinglePageJS"), Description("A JavaScript framework that only supports singlepage websites cause that's radder")]
        SPJS,
        [DisplayName("MultiPageJS"), Description("A JavaScript framework that only supports multipage websites cause that's rad")]
        MPJS
    };


}

