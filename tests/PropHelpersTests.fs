module PropHelperTests

open Fable.Mocha
open Feliz
open System

let propHelpersTests = testList "PropHelpers Tests" [

    testCase "createClockValue returns a valid result" <| fun _ ->
        Expect.equal "00:00:15.01" (PropHelpers.createClockValue(TimeSpan(0,0,0,15,1))) "The result is correct"
        Expect.equal "13:21:00.00" (PropHelpers.createClockValue(TimeSpan(13,21,0))) "The result is correct"
        Expect.equal "23:00:50.00" (PropHelpers.createClockValue(TimeSpan(23,0,50))) "The result is correct"
        Expect.equal "00:08:34.20" (PropHelpers.createClockValue(TimeSpan(0,0,8,34,20))) "The result is correct"
        
    testCase "createKeySplines returns a valid result" <| fun _ ->
        let expected = "0 1.5 2 3; 4 5 6.5 7"

        let actual = PropHelpers.createKeySplines([0.,1.5,2.,3.; 4.,5.,6.5,7.])
        
        Expect.equal expected actual "The result is correct"

    testCase "createPoints returns a valid result" <| fun _ ->
        let expected = "1,2 3,4"
        
        let actualFloat = PropHelpers.createPointsFloat([1.,2.;3.,4.])
        let actualInt = PropHelpers.createPointsInt([1,2;3,4])
        
        Expect.equal expected actualFloat "The result is correct"
        Expect.equal expected actualInt "The result is correct"
        
    testCase "createSvgPath returns a valid result" <| fun _ ->
        let expected = 
            [ "M 10,30" 
              "A 20,20 0,0,1 50,30"
              "A 20,20 0,0,1 90,30"
              "Q 90,60 50,90"
              "Q 10,60 10,30" 
              "z" ]
            |> String.concat System.Environment.NewLine
        
        let actualFloat = 
            PropHelpers.createSvgPathFloat([
                'M', [[10.;30.]]
                'A', [[20.;20.]; [0.;0.;1.]; [50.;30.]]
                'A', [[20.;20.]; [0.;0.;1.]; [90.;30.]]
                'Q', [[90.;60.]; [50.;90.]]
                'Q', [[10.;60.]; [10.;30.]]
                'z', []
            ])
        let actualInt =
            PropHelpers.createSvgPathInt([
                'M', [[10;30]]
                'A', [[20;20]; [0;0;1]; [50;30]]
                'A', [[20;20]; [0;0;1]; [90;30]]
                'Q', [[90;60]; [50;90]]
                'Q', [[10;60]; [10;30]]
                'z', []
            ])
        
        Expect.equal expected actualFloat "The result is correct"
        Expect.equal expected actualInt "The result is correct"
]
