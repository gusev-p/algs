cmake_minimum_required(VERSION 3.8)
project(cpp2)

set(CMAKE_CXX_STANDARD 14)

set(SOURCE_FILES
        problems/ExtraLongFactorials.cpp
        problems/NonDivisibleSubset.cpp
        problems/QueensAttack2.cpp
)
add_executable(cpp2 ${SOURCE_FILES})