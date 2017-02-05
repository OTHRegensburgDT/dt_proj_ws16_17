################################################################################
# Automatically-generated file. Do not edit!
################################################################################

# Add inputs and outputs from these tool invocations to the build variables 
C_SRCS += \
../MCUInit/ccu4.c \
../MCUInit/ccu8.c \
../MCUInit/dac.c \
../MCUInit/gpio.c \
../MCUInit/nvic.c \
../MCUInit/posif.c \
../MCUInit/systick.c \
../MCUInit/vadc.c 

OBJS += \
./MCUInit/ccu4.o \
./MCUInit/ccu8.o \
./MCUInit/dac.o \
./MCUInit/gpio.o \
./MCUInit/nvic.o \
./MCUInit/posif.o \
./MCUInit/systick.o \
./MCUInit/vadc.o 

C_DEPS += \
./MCUInit/ccu4.d \
./MCUInit/ccu8.d \
./MCUInit/dac.d \
./MCUInit/gpio.d \
./MCUInit/nvic.d \
./MCUInit/posif.d \
./MCUInit/systick.d \
./MCUInit/vadc.d 


# Each subdirectory must supply rules for building sources it contributes
MCUInit/%.o: ../MCUInit/%.c
	@echo 'Building file: $<'
	@echo 'Invoking: ARM-GCC C Compiler'
	"$(TOOLCHAIN_ROOT)/bin/arm-none-eabi-gcc" -MMD -MT "$@" -DXMC4800_F144x2048 -I"$(PROJECT_LOC)/Libraries/XMCLib/inc" -I"$(PROJECT_LOC)/MCUInit" -I"$(PROJECT_LOC)/MidSys" -I"$(PROJECT_LOC)/Libraries/CMSIS/Include" -I"$(PROJECT_LOC)/Libraries/CMSIS/Infineon/XMC4800_series/Include" -I"$(PROJECT_LOC)" -I"$(PROJECT_LOC)/Libraries" -I"$(PROJECT_LOC)/sensor" -I"$(PROJECT_LOC)/posif" -I"$(PROJECT_LOC)/common" -I"$(PROJECT_LOC)/motor" -O0 -ffunction-sections -fdata-sections -Wall -std=gnu99 -mfloat-abi=softfp -Wa,-adhlns="$@.lst" -pipe -c -fmessage-length=0 -MMD -MP -MF"$(@:%.o=%.d)" -MT"$(@:%.o=%.d) $@" -mcpu=cortex-m4 -mfpu=fpv4-sp-d16 -mthumb -g -gdwarf-2 -o "$@" "$<" 
	@echo 'Finished building: $<'
	@echo.

