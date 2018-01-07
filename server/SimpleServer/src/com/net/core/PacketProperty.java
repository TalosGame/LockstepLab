package com.net.core;

public enum PacketProperty {
    Unreliable,         	//0
    Reliable,               //1
    Sequenced,              //2
    ReliableOrdered,        //3
    AckReliable,            //4
    AckReliableOrdered,     //5
    Ping,                   //6
    Pong,                   //7
    ConnectRequest,         //8
    ConnectAccept,          //9
    Disconnect,             //10
    UnconnectedMessage,     //11
    NatIntroductionRequest, //12
    NatIntroduction,        //13
    NatPunchMessage,        //14
    MtuCheck,               //15
    MtuOk,                  //16
    DiscoveryRequest,       //17
    DiscoveryResponse,      //18
    Merged                  //19
}