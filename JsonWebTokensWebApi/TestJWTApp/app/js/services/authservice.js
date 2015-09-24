'use strict';

var servicesModule = require('./_index.js');

/**
 * @ngInject
 */
function AuthService($window) {
 	var service = {};

  	service.parseJwt = function(token){
		var base64Url = token.split('.')[1];
		var base64 = base64Url.replace('-', '+').replace('_', '/');
		return JSON.parse($window.atob(base64));
	};
	
	service.saveToken = function(token){
		$window.localStorage['JWT'] = token;
	};
	
	service.getToken = function(){
		return $window.localStorage['JWT'];
	};
	
	service.isAuthenticated = function(){
		var token = service.getToken();
		if(token) {
			var params = service.parseJwt(token);
			return Math.round(new Date().getTime() / 1000) <= params.exp;
		} else {
			return false;
		}
	};
	
	service.getParsedJwt = function(){
		var token = service.getToken();
		return token ? service.parseJwt(token) : '';
	}
		
	service.logout = function(){
		$window.localStorage.removeItem('JWT');
	};
	
  	return service;

}

servicesModule.service('authService', AuthService);