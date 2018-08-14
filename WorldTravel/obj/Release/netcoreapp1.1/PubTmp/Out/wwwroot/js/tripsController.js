//tripsController.js 

(function () {

    "use strict"; 


    //Getting existing module
    angular.module("app-trips")
        .controller("tripsController", tripsController);

    function tripsController($http) {

        var vm = this; 


        vm.trips = []; 

        vm.newTrip = {}; 

        vm.errorMessage = ""; 

        vm.isBusy = true; 


        $http.get('/api/trips')
            .then(function (response) {
                //success

                angular.copy(response.data, vm.trips);


            }, function (error) {
                //failure
                vm.errorMessage = "Failed To Load " + error;
            })
            .finally(function () {


                vm.isBusy = false; 
            });

        vm.addTrip = function () {



            vm.isBusy = true; 

            vm.errorMessage = ""; 

            $http.post('/api/trips', vm.newTrip)
                .then(function (response) {
                    //Success
                    vm.trips.push(response.data); 
                    vm.newTrip = {}; 

                }, function () {
                    //failure
                    vm.errorMessage = "Failed to save the New trip";
                    
                })
                .finally(function () {

                    vm.isBusy = false; 
                });
        };


        vm.deleteTrip = function (trip) {

            //const index = vm.trips.indexOf(trip);
            if (confirm("Are you sure want to remove " + trip.name + "?")) {
                vm.isBusy = true;
                vm.errorMessage = "";
                $http.get('/api/trips/delete/' + encodeURI(trip.name))
                    .then(function () {
                        $http.get('/api/trips')
                            .then(function (response) {
                                //success

                                angular.copy(response.data, vm.trips);


                            }, function (error) {
                                //failure
                                vm.errorMessage = "Failed To Load " + error;
                            })
                            .finally(function () {


                                vm.isBusy = false;
                            });


                    }, function () {
                        vm.errorMessage = "Failed to delete the trip";

                    })
                    .finally(function () {

                        vm.isBusy = false;


                    });
            }
            

        }



    }


})();