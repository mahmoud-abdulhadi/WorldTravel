//tripEditorController.js
(function () {
    "use strict";


   
    

    angular.module("app-trips")
        .controller("tripEditorController", tripEditorController)
    function tripEditorController($routeParams,$http) {

        var vm = this; 

        vm.tripName = $routeParams.tripName; 


        vm.stops = []; 


        vm.input = document.getElementById('name');

        var autocomplete = new google.maps.places.Autocomplete(vm.input);

        vm.errorMessage = ""; 

        vm.isBusy = true; 


        vm.newStop = {}; 

        var url = "/api/trips/" + encodeURI(vm.tripName) + "/stops"; 
    
        $http.get(url)
            .then(function (response) {

                //success

                angular.copy(response.data, vm.stops);

                var map = _showMap(vm.stops);

                
                
                    
               

                


            }, function (error) {
                //Failure

                vm.errorMessage = "Failed to load stops "; 

            })
            .finally(function () {

                vm.isBusy = false;

            });

        vm.addStop = function() {

           
            
            vm.isBusy = true; 


            $http.post(url, vm.newStop)
                .then(function (response) {
                    //success
                    vm.stops.push(response.data);
                    _showMap(vm.stops);
                    vm.newStop = {};

                }, function (error) {
                    //failure
                    vm.errorMessage = "Failed to add new stop"; 
                })
                .finally(function () {

                    vm.isBusy = false; 

                });

            

                    

        }

        vm.deleteStop = function (stop) {


            //find the index of the stop 
           // const index = vm.stops.indexOf(stop);



            if (confirm('Are You Sure,You want to remove ' + stop.name + ' Stop?')) {
                vm.isBusy = true; 

                vm.errorMessage = "";

                $http.get('/api/trips/' + encodeURI(vm.tripName) + '/stops/delete/' + encodeURI(stop.name))
                    .then(function () {
                        $http.get(url)
                            .then(function (response) {

                                //success

                                angular.copy(response.data, vm.stops);

                                var map = _showMap(vm.stops);





                            }, function (error) {
                                //Failure

                                vm.errorMessage = "Failed to load stops ";

                            })
                            .finally(function () {

                                vm.isBusy = false;

                            });

                    },

                    function () {

                        vm.errorMessage = "Failed to Delete The Stop";
                    }
                )
                    .finally(function () {


                        vm.isBusy = false; 
                    });
            }
               


        }

        vm.setCurrent = function (stop) {
            var index = vm.stops.indexOf(stop);

            _showMap(vm.stops, index);


        }

    }

    function _showMap(stops,index = 0) {

       
        if (stops && stops.length > 0) {

            var mapStops = _.map(stops, function (item) {
                return {

                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name

                };

            });

            //Show Map 
            return travelMap.createMap({

                stops: mapStops,
                selector: "#map",
                currentStop: index,
                initialZoom: 3

            });


        }         
        

    }

    
   

    

})();





