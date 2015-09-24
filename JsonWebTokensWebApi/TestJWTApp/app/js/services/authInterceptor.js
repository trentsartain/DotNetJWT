  'use strict'
  
  var servicesModule = require('./_index.js');
  /**
 * @ngInject
 */
function AuthInterceptor($q,authService) {
 	    var service = {};
     
      service.request = function(config){
        var token = authService.getToken();
          if(config.url.indexOf('http://localhost:2407') === 0 && token) {
            config.headers.Authorization = 'Bearer ' + token;
          }
        
          return config;
      };
      
      service.response = function(res){
        if(res.config.url.indexOf('http://localhost:2194') === 0 && res.data.access_token) {
            authService.saveToken(res.data.access_token);
          }
        
          return res;
      };
      
      service.responseError = function(res){return $q.reject(res); };
	
  	return service;
}

servicesModule.service('authInterceptor', AuthInterceptor);