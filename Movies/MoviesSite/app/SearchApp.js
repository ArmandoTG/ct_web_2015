﻿var moviesapp = angular.module("moviesapp", ['ngRoute', 'kendo.directives']);
    $routeProvider.when('/default',
            {
                templateUrl: 'views/search.html',
                controller : 'SearchController'
            }
        ).otherwise({ redirectTo: '/default' });
    }