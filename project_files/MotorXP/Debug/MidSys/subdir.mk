################################################################################
# Automatically-generated file. Do not edit!
################################################################################

# Add inputs and outputs from these tool invocations to the build variables 
C_SRCS += \
../MidSys/bldc_scalar_current_motor.c \
../MidSys/bldc_scalar_pwm_bc.c \
../MidSys/bldc_scalar_speed_pos_hall.c \
../MidSys/bldc_scalar_volt_dcbus.c \
../MidSys/bldc_scalar_volt_potentiometer.c \
../MidSys/bldc_scalar_volt_userdef.c 

OBJS += \
./MidSys/bldc_scalar_current_motor.o \
./MidSys/bldc_scalar_pwm_bc.o \
./MidSys/bldc_scalar_speed_pos_hall.o \
./MidSys/bldc_scalar_volt_dcbus.o \
./MidSys/bldc_scalar_volt_potentiometer.o \
./MidSys/bldc_scalar_volt_userdef.o 

C_DEPS += \
./MidSys/bldc_scalar_current_motor.d \
./MidSys/bldc_scalar_pwm_bc.d \
./MidSys/bldc_scalar_speed_pos_hall.d \
./MidSys/bldc_scalar_volt_dcbus.d \
./MidSys/bldc_scalar_volt_potentiometer.d \
./MidSys/bldc_scalar_volt_userdef.d 


# Each subdirectory must supply rules for building sources it contributes
MidSys/%.o: ../MidSys/%.c
	@echo 'Building file: $<'
	@echo 'Invoking: ARM-GCC C Compiler'
	"$(TOOLCHAIN_ROOT)/bin/arm-none-eabi-gcc" -MMD -MT "$@" -DXMC4800_F144x2048 -I"$(PROJECT_LOC)/Libraries/XMCLib/inc" -I"$(PROJECT_LOC)/MCUInit" -I"$(PROJECT_LOC)/MidSys" -I"$(PROJECT_LOC)/Libraries/CMSIS/Include" -I"$(PROJECT_LOC)/Libraries/CMSIS/Infineon/XMC4800_series/Include" -I"$(PROJECT_LOC)" -I"$(PROJECT_LOC)/Libraries" -I"$(PROJECT_LOC)/sensor" -I"$(PROJECT_LOC)/posif" -I"$(PROJECT_LOC)/common" -I"$(PROJECT_LOC)/motor" -O0 -ffunction-sections -fdata-sections -Wall -std=gnu99 -mfloat-abi=softfp -Wa,-adhlns="$@.lst" -pipe -c -fmessage-length=0 -MMD -MP -MF"$(@:%.o=%.d)" -MT"$(@:%.o=%.d) $@" -mcpu=cortex-m4 -mfpu=fpv4-sp-d16 -mthumb -g -gdwarf-2 -o "$@" "$<" 
	@echo 'Finished building: $<'
	@echo.

