function [ Energie, Ba, Bb, Bc ] = calcEnergieAndMagneticField_m( Ua, Ub, Uc, Angle )
%todo: siehe blatt
%    
    %variables that have to be specified
    coilResistence = 13; %Ohm
    coilConstant = 12; % n/l    n=wicklung, l= länge in m
    bStator = 1;  %H
    
    %check which case of stator current feed is applied -> see documantion
    %for more information 
    case1 = 
    
    
    
    
    
    
    %fill stator magnetic field
    stator = [ bStator, bStator, bStator, -bStator, -bStator, -bStator, bStator, bStator, bStator, -bStator, -bStator, -bStator, bStator, bStator, bStator, -bStator, -bStator, -bStator, bStator, bStator, bStator, -bStator, -bStator, -bStator, bStator, bStator, bStator, -bStator, -bStator, -bStator, bStator, bStator, bStator, -bStator, -bStator, -bStator, bStator, bStator, bStator, -bStator, -bStator, -bStator];
             
    %identify magnetic field over coil, first 7 are A/B/C second 7 are
    %notA/notB/notC
    BOverA = zeros(1, 14);
    BOverB = zeros(1, 14);
    BOverC = zeros(1, 14);
    
    startOffset = mod(round(Angle/(360/42)), 42)+1;
    runner = startOffset;
    coil = 1;
    indexOffset = 0;
    switchIndexOffset = mod(startOffset+21, 42)+1; %die Stelle ab der die Array Stellen 8-14 befüllt werden müssen
    fillIndex = 1;
    
    
    while 1 %simuliere do-while schleife
        switch (coil)
           case 1
              BOverA(fillIndex+indexOffset) = stator(runner);
           case 2
              BOverB(fillIndex+indexOffset) = stator(runner);
           case 3
              BOverC(fillIndex+indexOffset) = stator(runner);
        end
        
        runner = mod((runner), 42)+1;
        
        if runner == switchIndexOffset
            indexOffset = 7;
        end
        
        fillIndex = mod(fillIndex, 7)+1;
        if fillIndex == 1
            coil = mod(coil, 3)+1;
        end
        
        %jetzt die Bedingung die im while stehen würde
        if runner== startOffset
            break
        end
    end

    %calc current pro Coil
    Ia = Ua/coilResistence;
    Ib = Ub/coilResistence;
    Ic = Uc/coilResistence;
    
    %fild pro Coil without sign
    hCoilA = coilConstant*Ia;
    hCoilB = coilConstant*Ib;
    hCoilC = coilConstant*Ic;
    
    %field pro Coil
    Ba = [hCoilA, hCoilA, hCoilA, hCoilA, hCoilA, hCoilA, hCoilA, -hCoilA, -hCoilA, -hCoilA, -hCoilA, -hCoilA, -hCoilA, -hCoilA];
    Bb = [hCoilB, hCoilB, hCoilB, hCoilB, hCoilB, hCoilB, hCoilB, -hCoilB, -hCoilB, -hCoilB, -hCoilB, -hCoilB, -hCoilB, -hCoilB];
    Bc = [hCoilC, hCoilC, hCoilC, hCoilC, hCoilC, hCoilC, hCoilC, -hCoilC, -hCoilC, -hCoilC, -hCoilC, -hCoilC, -hCoilC, -hCoilC];
    
    %overlap of Coil with magnetic field
    overlapA = Ba+BOverA;
    overlapB = Bb+BOverB;
    overlapC = Bc+BOverC;
    
    Energie = sum(overlapA) + sum(overlapB) + sum(overlapC);
    
    
    
    
    
    
end

