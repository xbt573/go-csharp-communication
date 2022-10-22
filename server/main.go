package main

import (
	"fmt"
	"log"
	"net"
	"net/rpc"
	"net/rpc/jsonrpc"
	"os"
)

type API struct{}

func (API) Hello(message string, reply *string) error {
	*reply = message
	return nil
}

func main() {
	*log.Default() = *log.New(os.Stdout, "", log.LstdFlags|log.Lshortfile)

	addr, err := net.ResolveTCPAddr("tcp", "localhost:0")
	if err != nil {
		log.Fatalln(err)
	}
	l, err := net.ListenTCP("tcp", addr)
	if err != nil {
		log.Fatalln(err)
	}
	defer l.Close()

	fmt.Println(l.Addr().(*net.TCPAddr).Port)

	api := API{}
	rpc.Register(api)

	for {
		conn, err := l.Accept()
		if err != nil {
			netErr := err.(net.Error)
			if netErr.Timeout() {
				continue
			}

			log.Println(err.Error())
			continue
		}

		go rpc.ServeCodec(jsonrpc.NewServerCodec(conn))
	}
}
