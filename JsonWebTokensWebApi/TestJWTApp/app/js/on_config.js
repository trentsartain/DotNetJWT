'use strict';

/**
 * @ngInject
 */
function OnConfig($stateProvider, $locationProvider, $urlRouterProvider, $httpProvider) {

  $locationProvider.html5Mode(true);
  $httpProvider.interceptors.push('authInterceptor');
  
  $stateProvider
  .state('Home', {
    url: '/',
    controller: 'HomeCtrl as home',
    templateUrl: 'home.html',
    title: 'Home'
  });

  $urlRouterProvider.otherwise('/');

}

module.exports = OnConfig;