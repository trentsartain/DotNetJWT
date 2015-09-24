'use strict';

var servicesModule = require('./_index.js');

/**
 * @ngInject
 */
function UserService($q, $http) {

  var service = {};

  service.login = function(username, password){
      var deferred = $q.defer();
      var requestData = {
        username: username,
        password: password,
        grant_type : 'password',
        client_id : 'd3c2e8f35db549df8b6507f7e025301d'
      };
      
      return $http({
        method: 'POST',
        url: 'http://localhost:2194/oauth2/token',
        data: requestData,
        headers: {'Content-Type': 'application/x-www-form-urlencoded'},
        transformRequest: function(obj) {
          var str = [];
          for(var p in obj)
          str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
          return str.join("&");
        }
      })
      .success(function(data) {
          deferred.resolve(data);
      })
      .error(function(err, status) {
        deferred.reject(err, status);
      });
    };
  return service;

}

servicesModule.service('userService', UserService);