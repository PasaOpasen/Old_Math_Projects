#ifndef _STRINGCONVERT_H
#define _STRINGCONVERT_H
#include <string>
#include <sstream>

template <typename T>
std::string toString(T val)
{
std::ostringstream oss;
oss<< val;
return oss.str();
}

template <typename T> 
T fromString(const std::string& s) 
{
std::istringstream iss(s);
T res;
iss >> res;
return res;
}

#endif /* _STRINGCONVERT_H */