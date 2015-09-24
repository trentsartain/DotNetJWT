'use strict';

var controllersModule = require('./_index');

/**
 * @ngInject
 */
function HomeCtrl($state, $http, userService, authService) {

  // ViewModel
  var vm = this;

  vm.title = 'JWT Testing';
  vm.loggedIn = authService.isAuthenticated();
  vm.claim = authService.getParsedJwt();
  vm.token = authService.getToken();
  vm.loginError = false;
  vm.message = '';
  
  vm.login = function(){      
      userService.login(vm.user.username, vm.user.password)
      .success(function(data){
          vm.loggedIn = true;
          vm.claim = authService.getParsedJwt();
          vm.token = authService.getToken();
          vm.loginError = false;
      })
      .error(function(err, status){
          vm.loggedIn = false;
          vm.loginError = true;
      });
  };
  
  vm.logout = function(){
    authService.logout();
    vm.loggedIn = false;
    vm.message = "";
  };
  
  vm.testApi = function(){
      $http({
        method: 'GET',
        url: 'http://localhost:2407/api/protected'
      })
      .success(function(data) {
          vm.message = data;
      })
      .error(function(err, status) {
        vm.message = [{message : status + ' - ' + err.Message}];
      });
  };
}

controllersModule.controller('HomeCtrl', HomeCtrl);