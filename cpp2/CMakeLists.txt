cmake_minimum_required(VERSION 3.8)
project(cpp2)

set(CMAKE_CXX_STANDARD 14)

set(SOURCE_FILES
        problems/ExtraLongFactorials.cpp
        problems/NonDivisibleSubset.cpp
        problems/QueensAttack2.cpp
        problems/OrganizingContainersOfBalls.cpp
        problems/Encryption.cpp
        problems/Pairs.cpp
        problems/HackerlandRadioTransmitters.cpp
        problems/Lcs.cpp
        common/common.cpp
)
include_directories(${PROJECT_SOURCE_DIR})
add_executable(cpp2 ${SOURCE_FILES})